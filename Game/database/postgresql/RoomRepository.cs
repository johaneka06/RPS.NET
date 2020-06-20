using System;
using Npgsql;
using NpgsqlTypes;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Game.Databases.PostgreSQL
{
    public class RoomRepository : IRoomRepository
    {
        private NpgsqlConnection _connection;
        private NpgsqlTransaction _transaction;

        public RoomRepository(NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }

        public void Create(Room room)
        {
            string query = "INSERT INTO room (roomId, maxPlayer) VALUES (@id, @max)";
            using (var cmd = new NpgsqlCommand(query, _connection, _transaction))
            {
                cmd.Parameters.AddWithValue("id", room.ID);
                cmd.Parameters.AddWithValue("max", room.MaxPlayer);
                cmd.ExecuteNonQuery();
            }
        }

        public void Join(Room room, Player player)
        {
            string query = "INSERT INTO play (roomId, userId) VALUES (@id, @player)";
            using (var cmd = new NpgsqlCommand(query, _connection, _transaction))
            {
                cmd.Parameters.AddWithValue("id", room.ID);
                cmd.Parameters.AddWithValue("player", player.ID);
                cmd.ExecuteNonQuery();
            }
        }

        public void Close(Room room)
        {
            string query = "UPDATE play SET deleted_at = CURRENT_TIMESTAMP WHERE roomId = @id AND deleted_at is null";
            using (var cmd = new NpgsqlCommand(query, _connection, _transaction))
            {
                cmd.Parameters.AddWithValue("id", room.ID);
                cmd.ExecuteNonQuery();
            }
        }

        public Room FindRoom(Guid id)
        {
            Room r;
            string query = "SELECT roomId, maxPlayer FROM room WHERE roomId = @id AND deleted_at is null";

            using(var cmd = new NpgsqlCommand(query, _connection, _transaction))
            {
                cmd.Parameters.AddWithValue("id", id);
                using(NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    if(reader.Read())
                    {
                        r = new Room(id, reader.GetInt32(1));
                        return r;
                    }
                }
            }
            return null;
        }

        public void ChangeGame(Room room, Casual game)
        {
            string query = "INSERT INTO game (gameId, roomId, game_name) VALUES (@gameId, @roomId, @name)";
            using(var cmd = new NpgsqlCommand(query, _connection, _transaction))
            {
                cmd.Parameters.AddWithValue("gameId", game.ID);
                cmd.Parameters.AddWithValue("roomId", room.ID);
                cmd.Parameters.AddWithValue("name", game.GameName());
                cmd.ExecuteNonQuery();
            }
        }

        public void Suwit(Room room, Suwit suwit)
        {
            string query = "INSERT INTO action (actionId, roomId, result, state) VALUES (@id, @room, @result, @state)";
            using (var cmd = new NpgsqlCommand(query, _connection, _transaction))
            {
                cmd.Parameters.AddWithValue("id", Guid.NewGuid());
                cmd.Parameters.AddWithValue("room", room.ID);

                string Json = JsonSerializer.Serialize(room.Game.GetMemento());
                cmd.Parameters.Add(new NpgsqlParameter("result", NpgsqlDbType.Jsonb) { Value = suwit });
                cmd.Parameters.Add(new NpgsqlParameter("state", NpgsqlDbType.Jsonb) { Value = Json });

                cmd.ExecuteNonQuery();
            }
        }
    }
}