using Application.Models;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IBookingService
    {
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task<Booking> GetBookingByIdAsync(int id);
        Task<Booking> AddBookingAsync(BookingCreateDTO bookingCreateDTO);
        Task UpdateBookingAsync(int id, BookingUpdateDTO bookingUpdateDTO);
        Task DeleteBookingAsync(int id);
    }
}
