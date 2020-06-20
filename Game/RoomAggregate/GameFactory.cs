using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Game
{
    public class GameFactory
    {
        public static Casual Create(string gameName, List<Guid> players, IPlayerRepository playerRepo = null, GameConfig config = null, string lastState = "")
        {
            if(config == null) config = GameConfig.Default();

            Casual gameResult;

            if(gameName.Equals("rock-paper-scissors"))
            {
                if(players.Count != 2) throw new Exception("Rock paper scissors must be played with 2 person!");
                
                gameResult = new RockPaperScissor(players);

                RPSResultHandler win = new WinHandler(playerRepo, new Multiplier(new RPSWin(), config.WinMultiplier));
                RPSResultHandler lose = new LoseHandler(playerRepo, new Multiplier(new RPSLose(), config.LoseMultiplier));

                gameResult.Attach(win);
                gameResult.Attach(lose);

                if(lastState == "" || lastState == null) return gameResult;

                if(gameName.Equals("rock-paper-scissors"))
                {
                    RPSMemento memento = JsonSerializer.Deserialize<RPSMemento>(lastState);
                    gameResult.LoadMemento(memento);
                }

                return gameResult;
            }
            else throw new Exception("Game not found!");
        }
    }

    public class GameConfig
    {
        public int WinMultiplier { get; set; }
        public int LoseMultiplier { get; set; }

        public static GameConfig Default()
        {
            return new GameConfig(1, 1);
        }

        public GameConfig(int win, int lose)
        {
            this.WinMultiplier = win;
            this.LoseMultiplier = lose;
        }

        public GameConfig() : this(1, 1) { }
    }
}