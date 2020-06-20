using System;

namespace Game
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
        void Rollback();
    }
}