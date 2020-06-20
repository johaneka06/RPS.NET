using System;
using System.Collections.Generic;

namespace Game
{
    public interface IPlayerRepository
    {
        void Create(Player p);
        Player FindPlayerByID(Guid id);
        void AddExp(Player p, int exp);
        List<Player> GetAllPlayers();
    }
}