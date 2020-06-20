using System;

namespace Game
{
    public interface IObserver<T>
    {
        void Update(T e);
    }

    public interface IOBservable<T>
    {
        void Attach(IObserver<T> obs);
        void Broadcast(T e);
    }
}