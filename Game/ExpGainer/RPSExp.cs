using System;

namespace Game
{
    public class RPSWin : IExpGainer
    {
        public int Gain()
        {
            return 5;
        }
    }

    public class RPSLose : IExpGainer
    {
        public int Gain()
        {
            return 2;
        }
    }
}