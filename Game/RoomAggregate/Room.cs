using System;
using System.Collections.Generic;

namespace Game
{
    public class Room
    {
        private Guid _id;
        private List<Guid> _players;
        private Casual _game;
        private int _maxPlayer;

        public Guid ID
        {
            get
            {
                return this._id;
            }
        }

        public List<Guid> Players
        {
            get
            {
                return this._players;
            }
        }

        public int MaxPlayer
        {
            get
            {
                return this._maxPlayer;
            }
        }

        public Casual Game
        {
            get
            {
                return this._game;
            }
        }

        public Room()
        {
            this._game = null;
            this._maxPlayer = 0;
            this._players = new List<Guid>();
        }

        public Room(Guid id, int max)
        {
            if (max > 1)
            {
                this._maxPlayer = max;
                this._players = new List<Guid>();
                this._id = id;
                this._game = null;
            }
            else throw new Exception("Maximum user must be morethan 1!");
        }

        public void Join(Player p)
        {
            if (_players.Count < _maxPlayer) _players.Add(p.ID);
            else if (_game != null) throw new Exception("Game has started!");
            else throw new Exception("Room is full!");
        }

        public void StartGame(string gameName, GameConfig config = null, IPlayerRepository repo = null)
        {
            _game = GameFactory.Create(gameName, _players, repo, config, "");
        }

        public void Suwit(Suwit s)
        {
            if(_game == null) throw new Exception("Game hasn't been started!");

            _game.Suwit(s);
        }

        public override bool Equals(object obj)
        {
            var o = obj as Room;
            if(o == null) return false;

            return o._id.Equals(this._id);
        }
        
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}