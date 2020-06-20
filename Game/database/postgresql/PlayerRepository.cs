using System;
using System.Collections.Generic;
using Npgsql;
using NpgsqlTypes;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Game.Databases.PostgreSQL
{
    public class PlayerRepository : IPlayerRepository
    {
        private NpgsqlConnection _connection;
        private NpgsqlTransaction _transaction;

        public PlayerRepository(NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }

        public void Create(Player p)
        {
            string query = "INSERT INTO \"player\" (playerId, playerName) VALUES (@id, @name)";
            using (var cmd = new NpgsqlCommand(query, _connection, _transaction))
            {
                cmd.Parameters.AddWithValue("id", p.ID);
                cmd.Parameters.AddWithValue("name", p.Name);
                cmd.ExecuteNonQuery();
            }
        }

        public Player FindPlayerByID(Guid id)
        {
            string name = "";
            string query = "SELECT playerName FROM \"player\" WHERE playerId = @id";
            using (var cmd = new NpgsqlCommand(query, _connection, _transaction))
            {
                cmd.Parameters.AddWithValue("id", id);
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        name = reader.GetString(0);
                    }
                }
            }
            Player p = new Player(id, name, new Exp(GetEXP(id)));

            return p;
        }

        public List<Player> GetAllPlayers()
        {
            List<Player> players = new List<Player>();
            string query = @"SELECT * FROM ""player"" p 
                LEFT JOIN
                    (SELECT playerId, SUM(xpvalue) as xp FROM exp GROUP BY playerId) e ON p.playerId = e.playerId";
            Player p;

            using(var cmd = new NpgsqlCommand(query, _connection, _transaction))
            {
                using(NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        p = new Player(reader.GetGuid(0), reader.GetString(1), new Exp(reader.GetInt32(2)));
                        players.Add(p);
                    }
                }
            }

            return players;
        }

        public void AddExp(Player p, int exp)
        {
            string query = "INSERT INTO exp (expid, playerid, xpvalue) VALUES (@id, @player, @xp)";
            using(var cmd = new NpgsqlCommand(query, _connection, _transaction))
            {
                cmd.Parameters.AddWithValue("id", Guid.NewGuid());
                cmd.Parameters.AddWithValue("player", p.ID);
                cmd.Parameters.AddWithValue("xp", exp);
                cmd.ExecuteNonQuery();
            }

            query = "SELECT COUNT(1) FROM exp WHERE playerid = @id";
            int count = 0;
            using(var cmd = new NpgsqlCommand(query, _connection, _transaction))
            {
                cmd.Parameters.AddWithValue("id", p.ID);
                using(NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    if(reader.Read())
                    {
                        count = reader.GetInt32(0);
                    }
                }
            }

            if(count % 100 == 0) CreateSnapshot(p.ID);
        }

        private int GetEXP(Guid id)
        {
            string query = "SELECT expvalue, lastSnapshotAt FROM \"exp_snapshot\" WHERE playerId = @id";
            NpgsqlDateTime lastCreatedOn = new NpgsqlDateTime(0);
            int sum = 0;

            using (var cmd = new NpgsqlCommand(query, _connection, _transaction))
            {
                cmd.Parameters.AddWithValue("id", id);
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        sum = reader.GetInt32(0);
                        lastCreatedOn = reader.GetTimeStamp(1);
                    }
                }
            }

            query = "SELECT coalesce(SUM(xpValue), 0) FROM exp WHERE playerId = @id AND created_at > @lastCreate";

            using(var cmd = new NpgsqlCommand(query, _connection, _transaction))
            {
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("lastCreate", lastCreatedOn);
                using(NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    sum += reader.GetInt32(0);
                }
            }

            return sum;
        }

        private void CreateSnapshot(Guid id)
        {
            string query = "SELECT expId, created_at FROM exp WHERE playerId = @id";
            Guid lastId;
            NpgsqlDateTime lastDate;

            using (var cmd = new NpgsqlCommand(query, _connection, _transaction))
            {
                cmd.Parameters.AddWithValue("id", id);
                using(NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    if(reader.Read())
                    {
                        lastId = reader.GetGuid(0);
                        lastDate = reader.GetTimeStamp(1);
                    }
                    else throw new Exception("last exp not found!");
                }
            }

            int sum = GetEXP(id);

            query = "INSERT INTO exp_snapshot (snapshotId, lastId, playerId, expValue, lastSnapshotAt) VALUES (@id, @lastId, @player, @value, @lastCreated)";

            using(var cmd = new NpgsqlCommand(query, _connection, _transaction))
            {
                cmd.Parameters.AddWithValue("id", Guid.NewGuid());
                cmd.Parameters.AddWithValue("lastId", lastId);
                cmd.Parameters.AddWithValue("player", id);
                cmd.Parameters.AddWithValue("value", sum);
                cmd.Parameters.AddWithValue("lastCreated", lastDate);
                cmd.ExecuteNonQuery();
            }
        }
    }
}