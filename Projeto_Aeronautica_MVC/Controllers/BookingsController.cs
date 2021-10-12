using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto_Aeronautica_MVC.Data;
using Projeto_Aeronautica_MVC.Data.Entities;
using Projeto_Aeronautica_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Projeto_Aeronautica_MVC.Controllers
{
    public class BookingsController : Controller
    {
        private readonly DataContext _context;
        private readonly IBookingRepository _bookingRepository;
        private readonly IFlightRepository _flightRepository;
        private readonly IAirplaneRepository _airplaneRepository;

        public BookingsController(DataContext context, IBookingRepository bookingRepository, IFlightRepository flightRepository,
            IAirplaneRepository airplaneRepository)
        {
            _context = context;
            _bookingRepository = bookingRepository;
            _flightRepository = flightRepository;
            _airplaneRepository = airplaneRepository;
        }

        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin") || User.IsInRole("Employee"))
            {
                return RedirectToAction("Account", "NotAuthorized");
            }

            var model = await _bookingRepository.GetBookingAsync(this.User.Identity.Name);
            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            if (User.IsInRole("Admin") || User.IsInRole("Employee"))
            {
                return RedirectToAction("Account", "NotAuthorized");
            }

            var model = await _bookingRepository.GetDetailTempsAsync(this.User.Identity.Name);
            return View(model);
        }

        public IActionResult AddFlight(int? id)
        {
            if (User.IsInRole("Admin") || User.IsInRole("Employee"))
            {
                return RedirectToAction("Account", "NotAuthorized");
            }

            var model = new AddTicketViewModel();

            model.Quantity = 1;

            if (id != null)
            {
                model.FlightId = id.Value;
            }
            model.Flights = _flightRepository.GetComboFlightDestiny();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddFlight(AddTicketViewModel model)
        {
            if (ModelState.IsValid)
            {
                string alphabet = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
                string columnLetter = "";
                for (int i = 0; i < model.ColumnId; i++)
                {
                    columnLetter = alphabet.Substring(i, 1);
                }

                model.SeatName = $"{columnLetter}{model.ColumnNumberId}";

                var bdt = _context.BookingDetails.Any(x => x.SeatName == model.SeatName);
                var bdt2 = _context.BookingDetailsTemp.Any(x => x.SeatName == model.SeatName && x.User.UserName == this.User.Identity.Name);
                
                if (bdt == true)
                {
                    TempData["AddBookingError"] = "This Seat is taken!";

                    return RedirectToAction("AddFlight");
                }
                else if (bdt2 == true )
                {
                    TempData["AddBookingError"] = "You have already picked this seat!";

                    return RedirectToAction("AddFlight");
                }

                var flight = await _flightRepository.GetByIdAsync(model.FlightId);

                if(flight.IsAvailable == false)
                {
                    TempData["AddBookingError"] = "This Flight is currently Unavaliable";

                    return RedirectToAction("AddFlight");
                }

                if(flight.AvaliableSeats == 0)
                {
                    TempData["AddBookingError"] = "This Flight has no seats left!";

                    return RedirectToAction("AddFlight");
                }

                await _bookingRepository.AddTicketToBookingAsync(model, this.User.Identity.Name);

                return RedirectToAction("Create");
            }
            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (User.IsInRole("Admin") || User.IsInRole("Employee"))
            {
                return RedirectToAction("Account", "NotAuthorized");
            }

            if (id == null)
            {
                return NotFound();
            }

            var bookingContext = _context.BookingDetails;

            if (bookingContext == null)
            {
                return NotFound();
            }

            List<BookingDetail> bookingDetails = new List<BookingDetail>().ToList();

            foreach (var item in bookingContext)
            {
                if(item.BookingId == id.Value)
                {
                    bookingDetails.Add(item);
                }
            }

            return View(bookingDetails);
        }

        public async Task<IActionResult> DeleteItem(int? id)
        {
            if (User.IsInRole("Admin") || User.IsInRole("Employee"))
            {
                return RedirectToAction("Account", "NotAuthorized");
            }

            if (id == null)
            {
                return NotFound();
            }

            await _bookingRepository.DeleteDetailTempAsync(id.Value);
            return RedirectToAction("Create");
        }

        public async Task<IActionResult> ConfirmBooking()
        {
            if (this.User.IsInRole("Admin") || this.User.IsInRole("Employee"))
            {
                return RedirectToAction("Account", "NotAuthorized");
            }

            var response = await _bookingRepository.ConfirmBookingAsync(this.User.Identity.Name);

            if (response)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Create");
        }

        [HttpPost]
        [Route("Bookings/GetColumnsAsync")]
        public async Task<JsonResult> GetColumnsAsync(int flightId)
        {
            if(flightId == 0)
            {
                return Json(flightId);
            }
            var flight = await _flightRepository.GetByIdAsync(flightId);
            var airplane = await _airplaneRepository.GetByIdAsync(flight.AirplaneId);
            List<string> columnLetters = new List<string>();
            columnLetters.Sort();
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
            int counter = 0;

            foreach (char c in alphabet)
            {
                if (counter < airplane.NumberOfColumns)
                {
                    columnLetters.Add(c.ToString());
                }
                counter++;
            }
            return Json(columnLetters);
        }

        [HttpPost]
        [Route("Bookings/GetColumnNumbersAsync")]
        public async Task<JsonResult> GetColumnNumbersAsync(int flightId)
        {
            var flight = await _flightRepository.GetByIdAsync(flightId);
            var airplane = await _airplaneRepository.GetByIdAsync(flight.AirplaneId);
            List<string> columnNumbers = new List<string>();

            for (int i = 1; i <= airplane.SeatsPerColumn; i++)
            {
                columnNumbers.Add(i.ToString());
            }

            return Json(columnNumbers);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (User.IsInRole("Admin") || User.IsInRole("Employee"))
            {
                return RedirectToAction("Account", "NotAuthorized");
            }

            if (id == null)
            {
                return NotFound();
            }

            var booking = await _bookingRepository.GetByIdAsync(id.Value);
            if (booking == null)
            {
                return NotFound();
            }

            try
            {
                await _bookingRepository.DeleteAsync(booking);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ViewBag.ErrorTitle = $"This reservation is currently in use...";
                ViewBag.ErrorMessage = $"Unable to delete this data due to its usage ramifications<br>" +
                    $"Try again later...";
            }
            return View("Error");
        }
    }
}
