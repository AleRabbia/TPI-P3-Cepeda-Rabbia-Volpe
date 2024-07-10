using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ActionResult<IEnumerable<BookingDTO>>> GetAll()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
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
        public async Task<ActionResult<BookingDTO>> GetById(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
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
        public async Task<ActionResult<BookingDTO>> CreateBooking(BookingCreateDTO bookingCreateDTO)
        {
            var booking = await _bookingService.AddBookingAsync(bookingCreateDTO);

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
        public async Task<ActionResult> Update(int id, BookingUpdateDTO bookingUpdateDTO)
        {
            try
            {
                await _bookingService.UpdateBookingAsync(id, bookingUpdateDTO);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _bookingService.DeleteBookingAsync(id);
            return NoContent();
        }
    }
}
