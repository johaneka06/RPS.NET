using System;

namespace Game
{
    public abstract class RPSResultHandler : IObserver<GameResult>
    {
        protected IExpGainer _gainer;
        protected IPlayerRepository _repository;

        public RPSResultHandler(IPlayerRepository repository, IExpGainer gainer)
        {
            _gainer = gainer;
            _repository = repository;
        }

        public abstract void Update(GameResult e);
    }

    public class WinHandler : RPSResultHandler
    {
        public WinHandler(IPlayerRepository repository, IExpGainer gainer) : base(repository, gainer) { }

        public override void Update(GameResult e)
        {
            if(_repository == null) return;


            Win evnt = e as Win;
            if(evnt == null) return;

            Player p = _repository.FindPlayerByID(e.Player);
            _repository.AddExp(p, _gainer.Gain());
        }
    }

    public class LoseHandler : RPSResultHandler
    {
        public LoseHandler(IPlayerRepository repository, IExpGainer gainer) : base(repository, gainer) { }

        public override void Update(GameResult e)
        {
            if(_repository == null) return;

            
            Lose evnt = e as Lose;
            if(evnt == null) return;

            Player p = _repository.FindPlayerByID(e.Player);
            _repository.AddExp(p, _gainer.Gain());
        }
    }
}