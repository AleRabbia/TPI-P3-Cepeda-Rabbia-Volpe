using Application.Models;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Interfaces
{
    public interface IBookingService
    {
        IEnumerable<Booking> GetAllBookings();
        Booking GetBookingById(int id);
        Booking AddBooking(BookingCreateDTO bookingCreateDTO);
        void UpdateBooking(int id, BookingUpdateDTO bookingUpdateDTO);
        void DeleteBooking(int id);
    }
}
