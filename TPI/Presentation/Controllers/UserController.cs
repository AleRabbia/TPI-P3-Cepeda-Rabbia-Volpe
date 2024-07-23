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

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserDto>> GetAll()
        {
            var users = _userService.GetAllUsers();
            var userDtos = users.Select(user => UserDto.Create(user));
            return Ok(userDtos);
        }

        [HttpGet("{id}")]
        public ActionResult<UserDto> GetById(int id)
        {
            var user = _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            var userDto = UserDto.Create(user);
            return Ok(userDto);
        }

        [HttpPost]
        public ActionResult Create(CreateUserDto userDto)
        {
            if (!Enum.TryParse(userDto.Role, out UserRole role))
            {
                return BadRequest("Invalid role.");
            }

            var user = new User
            {
                Name = userDto.Name,
                LastName = userDto.LastName,
                Email = userDto.Email,
                Password = userDto.Password,
                Birthdate = userDto.Birthdate,
                Role = role
            };

            _userService.AddUser(user);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, UserDto.Create(user));
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "UserOrAdmin")]
        public ActionResult Update(int id, UpdateUserDto userDto)
        {
            if (!Enum.TryParse(userDto.Role, out UserRole role))
            {
                return BadRequest("Invalid role.");
            }

            var existingUser = _userService.GetUserById(id);
            if (existingUser == null)
            {
                return NotFound("User not found.");
            }

            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (!User.IsInRole("Admin") && currentUserId != id)
            {
                return Forbid("You do not have permission to update this user.");
            }

            var user = new User
            {
                Id = id,
                Name = userDto.Name,
                LastName = userDto.LastName,
                Email = userDto.Email,
                Password = userDto.Password,
                Birthdate = userDto.Birthdate,
                Role = role
            };

            _userService.UpdateUser(user);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _userService.DeleteUser(id);
            return NoContent();
        }
    }
}
