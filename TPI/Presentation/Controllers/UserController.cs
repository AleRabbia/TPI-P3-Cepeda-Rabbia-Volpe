using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentation.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [Authorize]

    public ActionResult<ICollection<UserDto>> GetAll()
    {
        int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
        var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

        if (userRole != nameof(UserRole.Admin))
            return Forbid();

        var users = _userService.GetAllUsers();
        var userDtos = users.Select(user => UserDto.Create(user)).ToList();
        return Ok(userDtos);
    }

    [HttpGet("{id}")]
    [Authorize]
    public ActionResult<UserDto> GetById(int id)
    {
        int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
        var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

        if (userRole != nameof(UserRole.Admin) && userId != id)
            return Forbid();

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
        //int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
        //var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

        //if (userRole != nameof(UserRole.Admin))
            //return Forbid();

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
            Role = userDto.Role
        };

        _userService.AddUser(user);
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, UserDto.Create(user));
    }

    [HttpPut("{id}")]
    [Authorize]
    public ActionResult Update(int id, UpdateUserDto userDto)
    {
        int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
        var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

        if (userRole != nameof(UserRole.Admin) && userRole != nameof(UserRole.Customer) && userId != id)
            return Forbid();


        if (!Enum.TryParse(userDto.Role, out UserRole role))
        {
            return BadRequest("Invalid role.");
        }

        var existingUser = _userService.GetUserById(id);
        if (existingUser == null)
        {
            return NotFound("User not found.");
        }

        existingUser.Name = userDto.Name;
        existingUser.LastName = userDto.LastName;
        existingUser.Email = userDto.Email;
        existingUser.Password = userDto.Password;
        existingUser.Birthdate = userDto.Birthdate;
        existingUser.Role = userDto.Role;

        _userService.UpdateUser(existingUser);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize]
    public ActionResult Delete(int id)
    {
        int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
        var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

        if(userRole != nameof(UserRole.Admin) && userRole != nameof(UserRole.Customer))
                return Forbid();

        var existingUser = _userService.GetUserById(id);
        if (existingUser == null)
        {
            return NotFound("User not found.");
        }

        _userService.DeleteUser(id);
        return NoContent();
    }
}
