using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Projeto_Aeronautica_MVC.Data.Entities;
using Projeto_Aeronautica_MVC.Helpers;
using Projeto_Aeronautica_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projeto_Aeronautica_MVC.Data
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IFlightRepository _flightRepository;

        public BookingRepository(DataContext context, IUserHelper userHelper,
            IFlightRepository flightRepository) : base(context)
        {
            _context = context;
            _userHelper = userHelper;
            _flightRepository = flightRepository;
        }

        public async Task AddTicketToBookingAsync(AddTicketViewModel model, string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);

            if(user == null)
            {
                return;
            }

            var flight = await _context.Flights.FindAsync(model.FlightId);

            if (flight == null)
            {
                return;
            }

            var bookingDetailTemp = await _context.BookingDetailsTemp
                .Where(odt => odt.User == user && odt.Flight == flight)
                .FirstOrDefaultAsync();

                bookingDetailTemp = new BookingDetailTemp
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    SeatName = model.SeatName,
                    Address = model.Address,
                    Nationality = model.Nationality,
                    PhoneNumber = model.PhoneNumber,
                    DepartureDate = flight.DepartureDate,
                    CityDestiny = flight.CityDestiny,
                    FlightDestiny = flight.FlightDestiny,
                    FlightId = flight.Id,
                    Price = flight.Price,
                    Quantity = model.Quantity,
                    User = user
                };

            _context.BookingDetailsTemp.Add(bookingDetailTemp);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> ConfirmBookingAsync(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if(user == null)
            {
                return false;
            }

            var bookingTmps = await _context.BookingDetailsTemp
                .Include(o => o.Flight)
                .Where(o => o.User == user)
                .ToListAsync();

            if(bookingTmps == null || bookingTmps.Count == 0)
            {
                return false;
            }
            
            int value = 0;
            foreach (var item in bookingTmps)
            {
                value += Convert.ToInt32(item.Price);
            }

            var details = bookingTmps.Select(o => new BookingDetail
            {
                FirstName = o.FirstName,
                LastName = o.LastName,
                Address = o.Address,
                Nationality = o.Nationality,
                PhoneNumber = o.PhoneNumber,
                SeatName = o.SeatName,
                FlightDestiny = o.FlightDestiny,
                CityDestiny = o.CityDestiny,
                DepartureDate = o.DepartureDate,
                Price = o.Price,
                FlightId = o.FlightId,
                Flight = o.Flight,
                Quantity = o.Quantity,
            }).ToList();

            var data = new DateTime?();
            var flightDestiny = "";
            var cityDestiny = "";
            var flightId = 0;

            foreach (var item in details)
            {
                data = item.DepartureDate;
                flightDestiny = item.FlightDestiny;
                cityDestiny = item.CityDestiny;
                flightId = item.FlightId;
            }

            var flight = await _flightRepository.GetByIdAsync(flightId);

            var booking = new Booking
            {
                FlightId = flight.Id,
                Value = value,
                Quantity = bookingTmps.Count,
                FlightDestiny = flightDestiny,
                CityDestiny = cityDestiny,
                DepartureDate = data,
                BookingDate = DateTime.UtcNow,
                User = user,
                Tickets = details
            };

            var bookingHistory = new BookingHistory
            {
                FlightId = flight.Id,
                Value = value,
                Quantity = bookingTmps.Count,
                FlightDestiny = flightDestiny,
                CityDestiny = cityDestiny,
                DepartureDate = data,
                BookingDate = DateTime.UtcNow,
                User = user,
            };

            if (flight.AvailableSeats < booking.Quantity)
            {
                return false;
            }

            flight.AvailableSeats = flight.AvailableSeats - Convert.ToInt32(booking.Quantity);

            var group = _context.Flights.First(g => g.Id == flightId);;

            _context.Entry(group).CurrentValues.SetValues(flight);

            await CreateAsync(booking);
            _context.BookingsHistory.Add(bookingHistory);
            _context.BookingDetailsTemp.RemoveRange(bookingTmps);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task DeleteDetailTempAsync(int id)
        {
            var bookingDetailTemp = await _context.BookingDetailsTemp.FindAsync(id);
            if(bookingDetailTemp == null)
            {
                return;
            }

            _context.BookingDetailsTemp.Remove(bookingDetailTemp);
            await _context.SaveChangesAsync();
        }

        public async Task DepartureBooking(DepartureViewModel model)
        {
            var booking = await _context.Bookings.FindAsync(model.Id);
            if(booking == null)
            {
                return;
            }

            booking.DepartureDate = model.DepartureDate;
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }

        public async Task<IQueryable<BookingDetailTemp>> GetDetailTempsAsync(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return null;
            }

            return _context.BookingDetailsTemp
                .Include(p => p.Flight)
                .Where(o => o.User == user)
                .OrderBy(o => o.Flight.FlightApparatus);
        }

        public async Task<IQueryable<Booking>> GetBookingAsync(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if(user == null)
            {
                return null;
            }

            if(await _userHelper.IsUserInRoleAsync(user, "Admin"))
            {
                return _context.Bookings
                    .Include(o => o.User)
                    .Include(o => o.Tickets)
                    .ThenInclude(o => o.Flight)
                    .OrderByDescending(o => o.BookingDate);
            }

            return _context.Bookings
                .Include(o => o.Tickets)
                .ThenInclude(o => o.Flight)
                .Where(o => o.User == user)
                .OrderByDescending(o => o.BookingDate);
        }

        public async Task<Booking> GetBookingAsync(int id)
        {
            return await _context.Bookings.FindAsync(id);
        }

        public async Task<IQueryable<BookingHistory>> GetBookingHistoryAsync(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return null;
            }

            if (await _userHelper.IsUserInRoleAsync(user, "Admin"))
            {
                return _context.BookingsHistory
                    .Include(o => o.User)
                    .Include(o => o.Tickets)
                    .OrderByDescending(o => o.BookingDate);
            }

            return _context.BookingsHistory
                .Include(o => o.Tickets)
                .Where(o => o.User == user)
                .OrderByDescending(o => o.BookingDate);
        }

        public async Task<BookingHistory> GetBookingHistoryAsync(int id)
        {
            return await _context.BookingsHistory.FindAsync(id);
        }

        public async Task ModifyBookingDetailTempQuantityAsync(int id, double quantity)
        {
            var bookingDetailTemp = await _context.BookingDetailsTemp.FindAsync(id);
            if (bookingDetailTemp == null)
            {
                return;
            }

            //bookingDetailTemp.Quantity += quantity;
            //if (bookingDetailTemp.Quantity > 0)
            //{
            //    _context.BookingDetailsTemp.Update(bookingDetailTemp);
                await _context.SaveChangesAsync();
            //}
        }
    }
}
