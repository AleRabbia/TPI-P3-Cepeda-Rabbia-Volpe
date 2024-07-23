using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<BookingDTO>> GetAll()
    {
        int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
        var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

        if (userRole != nameof(UserRole.Admin))
            return Forbid();

        var bookings = _bookingService.GetAllBookings();
        var bookingDTOs = new List<BookingDTO>();
        foreach (var booking in bookings)
        {
            bookingDTOs.Add(new BookingDTO
            {
                Id = booking.Id,
                CustomerId = booking.CustomerId,
                RoomId = booking.RoomId,
                CheckinDate = booking.CheckinDate,
                CheckoutDate = booking.CheckoutDate
            });
        }
        return Ok(bookingDTOs);
    }

    [HttpGet("{id}")]
    public ActionResult<BookingDTO> GetById(int id)
    {
        int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
        var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

        if (userRole != nameof(UserRole.Admin))
            return Forbid();

        var booking = _bookingService.GetBookingById(id);
        if (booking == null)
        {
            return NotFound();
        }
        var bookingDTO = new BookingDTO
        {
            Id = booking.Id,
            CustomerId = booking.CustomerId,
            RoomId = booking.RoomId,
            CheckinDate = booking.CheckinDate,
            CheckoutDate = booking.CheckoutDate
        };
        return Ok(bookingDTO);
    }

    [HttpPost]
    public ActionResult<BookingDTO> CreateBooking(BookingCreateDTO bookingCreateDTO)
    {
        int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
        var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

        if (userRole != nameof(UserRole.Admin))
            return Forbid();

        var booking = _bookingService.AddBooking(bookingCreateDTO);

        var bookingDTO = new BookingDTO
        {
            Id = booking.Id,
            CustomerId = booking.CustomerId,
            RoomId = booking.RoomId,
            CheckinDate = booking.CheckinDate,
            CheckoutDate = booking.CheckoutDate
        };

        return CreatedAtAction(nameof(GetById), new { id = bookingDTO.Id }, bookingDTO);
    }

    [HttpPut("{id}")]
    public ActionResult Update(int id, BookingUpdateDTO bookingUpdateDTO)
    {
        int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
        var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

        if (userRole != nameof(UserRole.Admin))
            return Forbid();

        try
        {
            _bookingService.UpdateBooking(id, bookingUpdateDTO);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
        var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

        if (userRole != nameof(UserRole.Admin))
            return Forbid();

        _bookingService.DeleteBooking(id);
        return NoContent();
    }
}
