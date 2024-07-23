using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<RoomDto>> GetAll()
        {
            var rooms = _roomService.GetAllRooms();

            var roomDtos = rooms.Select(room => new RoomDto
            {
                Id = room.Id,
                Price = room.Price,
                Score = room.Score,
                Service = room.Service,
                Category = room.Category.ToString(), // Convert enum to string
                Occupation = room.Occupation
            });

            return Ok(roomDtos);
        }

        [HttpGet("{id}")]
        public ActionResult<RoomDto> GetById(int id)
        {
            var room = _roomService.GetRoomById(id);
            if (room == null)
            {
                return NotFound();
            }

            var roomDto = new RoomDto
            {
                Id = room.Id,
                Price = room.Price,
                Score = room.Score,
                Service = room.Service,
                Category = room.Category.ToString(), // Convert enum to string
                Occupation = room.Occupation
            };

            return Ok(roomDto);
        }

        [HttpPost]
        public ActionResult Create(CreateRoomDto roomDto)
        {
            if (!Enum.TryParse(roomDto.Category, out CategoryRoom category))
            {
                return BadRequest("Invalid category value.");
            }

            var room = new Room
            {
                Price = roomDto.Price,
                Score = roomDto.Score,
                Service = roomDto.Service,
                Category = category,
                Occupation = roomDto.Occupation
            };

            _roomService.AddRoom(room);
            return CreatedAtAction(nameof(GetById), new { id = room.Id }, room);
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, UpdateRoomDto roomDto)
        {
            if (!Enum.TryParse(roomDto.Category, out CategoryRoom category))
            {
                return BadRequest("Invalid category value.");
            }

            var room = new Room
            {
                Id = id, // Ensure ID is set correctly
                Price = roomDto.Price,
                Score = roomDto.Score,
                Service = roomDto.Service,
                Category = category,
                Occupation = roomDto.Occupation
            };

            _roomService.UpdateRoom(room);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _roomService.DeleteRoom(id);
            return NoContent();
        }
    }
}
