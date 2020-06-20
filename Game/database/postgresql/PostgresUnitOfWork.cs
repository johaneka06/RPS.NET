using System;
using Npgsql;
using NpgsqlTypes;

namespace Game.Databases.PostgreSQL
{
    public class PostgresUnitOfWork : IUnitOfWork
    {
        private NpgsqlConnection _connection;
        private NpgsqlTransaction _transaction;

        private IPlayerRepository _playerRepository;
        private IRoomRepository _roomRepository;

        public PostgresUnitOfWork(string connectionStr)
        {
            _connection = new NpgsqlConnection(connectionStr);
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        public IPlayerRepository PlayerRepository
        {
            get
            {
                if(this._playerRepository == null) _playerRepository = new PlayerRepository(_connection, _transaction);
                return this._playerRepository;
            }
        }

        public IRoomRepository RoomRepository
        {
            get
            {
                if(this._roomRepository == null) _roomRepository = new RoomRepository(_connection, _transaction);
                return this._roomRepository;
            }
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        private bool disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if(!this.disposed)
            {
                if(disposing)
                {
                    _connection.Close();
                }
                disposed = true;
            }
        }
    }
}