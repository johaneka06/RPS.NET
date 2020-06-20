using System;
using System.Collections.Generic;

namespace Game
{
    public abstract class Casual : IOBservable<GameResult>
    {
        private Guid _id;

        public Guid ID
        {
            get
            {
                return this._id;
            }
        }

        public Casual()
        {
            this._id = Guid.NewGuid();
        }

        public abstract void Suwit(Suwit s);
        public abstract string GameName();
        public abstract object GetMemento();
        public abstract void LoadMemento(object memento);

        protected List<IObserver<GameResult>> _observer = new List<IObserver<GameResult>>();

        public void Attach(IObserver<GameResult> obs)
        {
            _observer.Add(obs);
        }

        public void Broadcast(GameResult evnt)
        {
            foreach (var obs in _observer)
            {
                obs.Update(evnt);
            }
        }

        public override bool Equals(object obj)
        {
            var o = obj as Casual;
            if (o == null) return false;

            return o._id.Equals(this._id);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public abstract class Suwit
    {
        protected Guid _player1;
        protected Guid _player2;

        public Guid Player1
        {
            get
            {
                return this._player1;
            }
        }

        public Guid Player2
        {
            get
            {
                return this._player2;
            }
        }

        public Suwit(Guid player1, Guid player2)
        {
            this._player1 = player1;
            this._player2 = player2;
        }
    }
}