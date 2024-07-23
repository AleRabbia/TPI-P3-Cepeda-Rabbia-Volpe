using Domain.Entities;
using System.Collections.Generic;

namespace Domain.Interfaces
{
    public interface IRoomRepository
    {
        IEnumerable<Room> GetAll();
        Room GetById(int id);
        void Add(Room room);
        void Update(Room room);
        void Delete(int id);
    }
}
