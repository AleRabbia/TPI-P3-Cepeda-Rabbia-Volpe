using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
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
        int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
        var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

        if (userRole != nameof(UserRole.Admin))
            return Forbid();

        var rooms = _roomService.GetAllRooms();

        var roomDtos = rooms.Select(room => new RoomDto
        {
            Id = room.Id,
            Price = room.Price,
            Score = room.Score,
            Service = room.Service,
            Category = room.Category.ToString(),
            Occupation = room.Occupation
        });

        return Ok(roomDtos);
    }

    [HttpGet("{id}")]
    public ActionResult<RoomDto> GetById(int id)
    {
        int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
        var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

        if (userRole != nameof(UserRole.Admin))
            return Forbid();

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
            Category = room.Category.ToString(),
            Occupation = room.Occupation
        };

        return Ok(roomDto);
    }

    [HttpPost]
    public ActionResult Create(CreateRoomDto roomDto)
    {
        int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
        var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

        if (userRole != nameof(UserRole.Admin))
            return Forbid();

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
        int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
        var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

        if (userRole != nameof(UserRole.Admin))
            return Forbid();

        if (!Enum.TryParse(roomDto.Category, out CategoryRoom category))
        {
            return BadRequest("Invalid category value.");
        }

        var existingRoom = _roomService.GetRoomById(id);
        if (existingRoom == null)
        {
            return NotFound("Room not found.");
        }

        existingRoom.Price = roomDto.Price;
        existingRoom.Score = roomDto.Score;
        existingRoom.Service = roomDto.Service;
        existingRoom.Category = category;
        existingRoom.Occupation = roomDto.Occupation;

        _roomService.UpdateRoom(existingRoom);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
        var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

        if (userRole != nameof(UserRole.Admin))
            return Forbid();

        var existingRoom = _roomService.GetRoomById(id);
        if (existingRoom == null)
        {
            return NotFound("Room not found.");
        }

        _roomService.DeleteRoom(id);
        return NoContent();
    }
}
