using System;

namespace Game
{
    public interface IRoomRepository
    {
        void Create(Room r);
        void Join (Room room, Player player);
        Room FindRoom(Guid id);
        void ChangeGame(Room room, Casual game);
        void Close(Room room);
        void Suwit(Room room, Suwit suwit);
    }
}