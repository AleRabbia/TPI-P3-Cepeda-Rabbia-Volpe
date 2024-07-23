using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace Application.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;

        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public IEnumerable<Room> GetAllRooms()
        {
            return _roomRepository.GetAll();
        }

        public Room GetRoomById(int id)
        {
            return _roomRepository.GetById(id);
        }

        public void AddRoom(Room room)
        {
            _roomRepository.Add(room);
        }

        public void UpdateRoom(Room room)
        {
            _roomRepository.Update(room);
        }

        public void DeleteRoom(int id)
        {
            _roomRepository.Delete(id);
        }
    }
}
