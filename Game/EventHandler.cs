using System;

namespace Game
{
    public abstract class RPSResultHandler : IObserver<GameResult>
    {
        protected IExpGainer _gainer;

        public RPSResultHandler(IExpGainer gainer)
        {
            this._gainer = gainer;
        }

        public abstract void Update(GameResult e);
    }

    public class WinHandler : RPSResultHandler
    {
        public WinHandler(IExpGainer gainer) : base(gainer) { }

        public override void Update(GameResult e)
        {
            //Check repo first


            Win evnt = e as Win;
            if(evnt == null) return;

            //Find user, add user exp via repo
        }
    }

    public class LoseHandler : RPSResultHandler
    {
        public LoseHandler(IExpGainer gainer) : base(gainer) { }

        public override void Update(GameResult e)
        {
            //Check repo first

            
            Lose evnt = e as Lose;
            if(evnt == null) return;

            //Find user, add user exp via repo
        }
    }
}