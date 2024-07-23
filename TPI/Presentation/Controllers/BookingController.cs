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
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<BookingDTO>> GetAll()
        {
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
        [Authorize]
        public ActionResult<BookingDTO> GetById(int id)
        {
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
        [Authorize]
        public ActionResult<BookingDTO> CreateBooking(BookingCreateDTO bookingCreateDTO)
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (userRole != nameof(UserRole.Admin) && userRole != nameof(UserRole.Customer))
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
        [Authorize]
        public ActionResult Update(int id, BookingUpdateDTO bookingUpdateDTO)
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (userRole != nameof(UserRole.Admin) && userRole != nameof(UserRole.Customer))
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
        [Authorize]
        public ActionResult Delete(int id)
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (userRole != nameof(UserRole.Admin) && userRole != nameof(UserRole.Customer))
                return Forbid();

            _bookingService.DeleteBooking(id);
            return NoContent();
        }
    }
}
