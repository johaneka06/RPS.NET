using System;

namespace Game
{
    public class Player
    {
        private Guid _id;
        private Exp _xp;
        private string _name;

        public Guid ID
        {
            get
            {
                return this._id;
            }
        }

        public int XP
        {
            get
            {
                return this._xp.XP;
            }
        }

        public string Name
        {
            get
            {
                return this._name;
            }
        }

        public Player()
        {
            this._name = "";
            this._xp = new Exp();
        }

        public Player(Guid id, string name, Exp xp)
        {
            this._name = name;
            this._id = id;
            this._xp = xp;
        }

        public static Player NewPlayer(string name)
        {
            return new Player(Guid.NewGuid(), name, new Exp());
        }

        public void AddExp(int exp)
        {
            this._xp = this._xp.AddExp(exp);
        }

        public override bool Equals(object obj)
        {
            var o = obj as Player;
            if (o == null) return false;

            return o._id.Equals(this._id);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
