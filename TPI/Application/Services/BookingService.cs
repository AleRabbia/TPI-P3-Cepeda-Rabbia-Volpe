using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoomRepository _roomRepository;

        public BookingService(IBookingRepository bookingRepository, IUserRepository userRepository, IRoomRepository roomRepository)
        {
            _bookingRepository = bookingRepository;
            _userRepository = userRepository;
            _roomRepository = roomRepository;
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await _bookingRepository.GetAllAsync();
        }

        public async Task<Booking> GetBookingByIdAsync(int id)
        {
            return await _bookingRepository.GetByIdAsync(id);
        }

        public async Task<Booking> AddBookingAsync(BookingCreateDTO bookingCreateDTO)
        {
            var user = await _userRepository.GetByIdAsync(bookingCreateDTO.CustomerId);
            if (user == null)
                throw new Exception("User not found");

            var room = await _roomRepository.GetByIdAsync(bookingCreateDTO.RoomId);
            if (room == null)
                throw new Exception("Room not found");

            var bookings = await _bookingRepository.GetAllAsync();
            if (bookings.Any(b => b.RoomId == bookingCreateDTO.RoomId &&
                                  b.CheckinDate < bookingCreateDTO.CheckoutDate &&
                                  b.CheckoutDate > bookingCreateDTO.CheckinDate))
            {
                throw new Exception("La Habitación no esta disponible para la fecha solicitada");
            }

            var booking = new Booking
            {
                CustomerId = bookingCreateDTO.CustomerId,
                RoomId = bookingCreateDTO.RoomId,
                CheckinDate = bookingCreateDTO.CheckinDate,
                CheckoutDate = bookingCreateDTO.CheckoutDate
            };

            await _bookingRepository.AddAsync(booking);
            return booking;
        }

        public async Task UpdateBookingAsync(int id, BookingUpdateDTO bookingUpdateDTO)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            if (booking == null)
                throw new Exception("Booking not found");

            var room = await _roomRepository.GetByIdAsync(bookingUpdateDTO.RoomId);
            if (room == null)
                throw new Exception("Room not found");

            var bookings = await _bookingRepository.GetAllAsync();
            if (bookings.Any(b => b.RoomId == bookingUpdateDTO.RoomId &&
                                  b.Id != id &&
                                  b.CheckinDate < bookingUpdateDTO.CheckoutDate &&
                                  b.CheckoutDate > bookingUpdateDTO.CheckinDate))
            {
                throw new Exception("La Habitación no esta disponible para la fecha solicitada");
            }

            booking.RoomId = bookingUpdateDTO.RoomId;
            booking.CheckinDate = bookingUpdateDTO.CheckinDate;
            booking.CheckoutDate = bookingUpdateDTO.CheckoutDate;

            await _bookingRepository.UpdateAsync(booking);
        }

        public async Task DeleteBookingAsync(int id)
        {
            await _bookingRepository.DeleteAsync(id);
        }
    }
}
