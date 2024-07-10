using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
        {
            var users = await _userService.GetAllUsersAsync();
            var userDtos = users.Select(user => UserDto.Create(user));
            return Ok(userDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var userDto = UserDto.Create(user);
            return Ok(userDto);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateUserDto userDto)
        {
            if (!Enum.TryParse(userDto.Role, out UserRole role))
            {
                return BadRequest("Rol ingresado no es correcto.");
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

            await _userService.AddUserAsync(user);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, UserDto.Create(user));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateUserDto userDto)
        {
            if (!Enum.TryParse(userDto.Role, out UserRole role))
            {
                return BadRequest("Rol ingresado no es correcto.");
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

            await _userService.UpdateUserAsync(user);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }
    }
}
