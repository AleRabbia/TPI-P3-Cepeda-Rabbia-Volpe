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
        public async Task<ActionResult<IEnumerable<RoomDto>>> GetAll()
        {
            var rooms = await _roomService.GetAllRoomsAsync();

            var roomDtos = rooms.Select(room => new RoomDto
            {
                Id = room.Id,
                Price = room.Price,
                Score = room.Score,
                Service = room.Service,
                Category = room.Category.ToString(), // Convierte el enum a string
                Occupation = room.Occupation
            });

            return Ok(roomDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoomDto>> GetById(int id)
        {
            var room = await _roomService.GetRoomByIdAsync(id);
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
                Category = room.Category.ToString(), // Convierte el enum a string
                Occupation = room.Occupation
            };

            return Ok(roomDto);
        }


        [HttpPost]
        public async Task<ActionResult> Create(CreateRoomDto roomDto)
        {
            if (!Enum.TryParse(roomDto.Category, out CategoryRoom category))
            {
                return BadRequest("Valor de categoria no es correcto.");
            }

            var room = new Room
            {
                Price = roomDto.Price,
                Score = roomDto.Score,
                Service = roomDto.Service,
                Category = category,
                Occupation = roomDto.Occupation
            };

            await _roomService.AddRoomAsync(room);
            return CreatedAtAction(nameof(GetById), new { id = room.Id }, room);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateRoomDto roomDto)
        {
            // Convierte la string de Category a enum CategoryRoom
            if (!Enum.TryParse(roomDto.Category, out CategoryRoom category))
            {
                return BadRequest("Valor de categoria no es correcto.");
            }

            var room = new Room
            {
                Id = id, // Asegúrate de que el ID esté establecido correctamente
                Price = roomDto.Price,
                Score = roomDto.Score,
                Service = roomDto.Service,
                Category = category, // Asigna el valor del enum convertido
                Occupation = roomDto.Occupation
            };

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
