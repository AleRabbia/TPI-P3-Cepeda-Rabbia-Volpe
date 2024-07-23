using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public IEnumerable<Booking> GetAllBookings()
        {
            return _bookingRepository.GetAll();
        }

        public Booking GetBookingById(int id)
        {
            return _bookingRepository.GetById(id);
        }

        public Booking AddBooking(BookingCreateDTO bookingCreateDTO)
        {
            var user = _userRepository.GetById(bookingCreateDTO.CustomerId);
            if (user == null)
                throw new Exception("User not found");

            var room = _roomRepository.GetById(bookingCreateDTO.RoomId);
            if (room == null)
                throw new Exception("Room not found");

            var bookings = _bookingRepository.GetAll();
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

            _bookingRepository.Add(booking);
            return booking;
        }

        public void UpdateBooking(int id, BookingUpdateDTO bookingUpdateDTO)
        {
            var booking = _bookingRepository.GetById(id);
            if (booking == null)
                throw new Exception("Booking not found");

            var room = _roomRepository.GetById(bookingUpdateDTO.RoomId);
            if (room == null)
                throw new Exception("Room not found");

            var bookings = _bookingRepository.GetAll();
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

            _bookingRepository.Update(booking);
        }

        public void DeleteBooking(int id)
        {
            _bookingRepository.Delete(id);
        }
    }
}
