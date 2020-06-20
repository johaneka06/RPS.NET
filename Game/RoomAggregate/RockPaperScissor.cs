using System;
using System.Collections.Generic;

namespace Game
{
    public class RockPaperScissor : Casual
    {
        protected bool _gameEndend;
        private Guid _player1, _player2;
        private int player1Win, player2Win;
        public RockPaperScissor(List<Guid> players) : base()
        {
            if (players.Count != 2) throw new Exception("Rock Paper Scissor must be played by 2 people");

            _player1 = players[0];
            _player2 = players[1];

            player1Win = 0;
            player2Win = 0;

            _gameEndend = false;
        }

        public int P1Win
        {
            get
            {
                return player1Win;
            }
        }

        public int P2Win
        {
            get
            {
                return player2Win;
            }
        }

        public override void Suwit(Suwit s)
        {
            if (_gameEndend) throw new Exception("Game already ended");

            RockPaperScissorSuwit swit = s as RockPaperScissorSuwit;
            if (swit == null) throw new Exception("Invalid Rock-Paper-Scissor suwit!");

            if (swit.Player1Suwit == swit.Player2Suwit) Console.WriteLine("Draw");
            else if (swit.Player1Suwit.Equals("rock") && swit.Player2Suwit.Equals("paper"))
            {
                Console.WriteLine("Player 2 wins");
                player2Win++;
            }
            else if (swit.Player1Suwit.Equals("rock") && swit.Player2Suwit.Equals("scissor"))
            {
                Console.WriteLine("Player 1 Wins");
                player1Win++;
            }
            else if (swit.Player1Suwit.Equals("paper") && swit.Player2Suwit.Equals("rock"))
            {
                Console.WriteLine("Player 1 wins");
                player1Win++;
            }
            else if (swit.Player1Suwit.Equals("paper") && swit.Player2Suwit.Equals("scissor"))
            {
                Console.WriteLine("Player 2 wins");
                player2Win++;
            }
            else if (swit.Player1Suwit.Equals("scissor") && swit.Player2Suwit.Equals("rock"))
            {
                Console.WriteLine("Player 2 wins");
                player2Win++;
            }
            else if (swit.Player1Suwit.Equals("scissor") && swit.Player2Suwit.Equals("paper"))
            {
                Console.WriteLine("Player 1 Wins");
                player1Win++;
            }

            if(IsEnd())
            {
                _gameEndend = true;
                
                Guid winner = (player1Win == 3) ? _player1 : _player2;
                Guid loser = (winner.Equals(_player1)) ? _player2 : _player1;

                Broadcast(new Win(winner));
                Broadcast(new Lose(loser));
            }

        }

        private bool IsEnd()
        {
            if(player1Win == 3 || player2Win == 3) return true;
            return false;
        }

        public override void LoadMemento(object memento)
        {
            var m = memento as RPSMemento;
            if (m == null) return;

            this._gameEndend = m.gameEnded;
            this.player1Win = m.player1Win;
            this.player2Win = m.player2Win;
            this._player1 = m.player1;
            this._player2 = m.player2;
        }

        public override object GetMemento()
        {
            return new RPSMemento(_gameEndend, _player1, _player2, player1Win, player2Win);
        }

        public override string GameName()
        {
            return ("Rock-Paper-Scissor");
        }
    }

    public class RockPaperScissorSuwit : Suwit
    {
        private string player1;
        private string player2;

        public string Player1Suwit
        {
            get
            {
                return this.player1;
            }
        }

        public string Player2Suwit
        {
            get
            {
                return this.player2;
            }
        }

        public RockPaperScissorSuwit(Guid player1, Guid player2, string player1Sw, string player2Sw) : base(player1, player2)
        {
            this.player1 = player1Sw;
            this.player2 = player2Sw;
        }

    }

    public class RPSMemento
    {
        public bool gameEnded { set; get; }
        public Guid player1 { set; get; }
        public Guid player2 { set; get; }
        public int player1Win { set; get; }
        public int player2Win { set; get; }

        public RPSMemento() { }

        public RPSMemento(bool GameEnded, Guid p1, Guid p2, int p1Win, int p2Win)
        {
            gameEnded = GameEnded;
            player1 = p1;
            player2 = p2;
            player1Win = p1Win;
            player2Win = p2Win;
        }
    }
}