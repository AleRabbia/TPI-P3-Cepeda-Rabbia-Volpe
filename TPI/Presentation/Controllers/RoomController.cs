using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<IEnumerable<Room>>> GetAll()
        {
            var rooms = await _roomService.GetAllRoomsAsync();
            return Ok(rooms);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> GetById(int id)
        {
            var room = await _roomService.GetRoomByIdAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            return Ok(room);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateRoomDto roomDto)
        {
            var room = new Room
            {
                Price = roomDto.Price,
                Score = roomDto.Score,
                Service = roomDto.Service,
                Category = roomDto.Category, // Asignación directa, roomDto.Category es CategoryRoom
                Occupation = roomDto.Occupation
            };

            await _roomService.AddRoomAsync(room);
            return CreatedAtAction(nameof(GetById), new { id = room.Id }, room);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Room room)
        {
            if (id != room.Id)
            {
                return BadRequest();
            }
            await _roomService.UpdateRoomAsync(room);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _roomService.DeleteRoomAsync(id);
            return NoContent();
        }
    }
}
