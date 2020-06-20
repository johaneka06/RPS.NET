using System;

namespace Game
{
    public class Exp
    {
        private int _value;

        public int XP
        {
            get
            {
                return this._value;
            }
        }

        public Exp()
        {
            this._value = 0;
        }

        public Exp(int value)
        {
            if (value < 0) throw new Exception("Value cannot be negative!");
            this._value = value;
        }

        public Exp AddExp(int value)
        {
            if (value <= 0) throw new Exception("Value must be more than 0!");
            return new Exp(this._value + value);
        }

        public override bool Equals(object obj)
        {
            var o = obj as Exp;
            if (o == null) return false;

            return o._value == this._value;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}