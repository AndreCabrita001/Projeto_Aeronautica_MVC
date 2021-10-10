using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto_Aeronautica_MVC.Data;
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
            var model = await _bookingRepository.GetBookingAsync(this.User.Identity.Name);
            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            var model = await _bookingRepository.GetDetailTempsAsync(this.User.Identity.Name);
            return View(model);
        }

        public IActionResult AddFlight()
        {
            var model = new AddTicketViewModel();
            model.Quantity = 1;
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

                var bdt = _context.BookingDetailsTemp.Any(x => x.SeatName == model.SeatName);

                if (bdt == true)
                {
                    TempData["AddBookingError"] = "This Seat is taken!";

                    return RedirectToAction("AddFlight");
                }

                await _bookingRepository.AddTicketToBookingAsync(model, this.User.Identity.Name);

                return RedirectToAction("Create");
            }
            return View(model);
        }

        public async Task<IActionResult> DeleteItem(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _bookingRepository.DeleteDetailTempAsync(id.Value);
            return RedirectToAction("Create");
        }


        public async Task<IActionResult> Increase(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _bookingRepository.ModifyBookingDetailTempQuantityAsync(id.Value, 1);
            return RedirectToAction("Create");
        }


        public async Task<IActionResult> Decrease(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _bookingRepository.ModifyBookingDetailTempQuantityAsync(id.Value, -1);
            return RedirectToAction("Create");
        }


        public async Task<IActionResult> ConfirmBooking()
        {
            var response = await _bookingRepository.ConfirmBookingAsync(this.User.Identity.Name);
            if (response)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Create");
        }


        public async Task<IActionResult> Departure(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _bookingRepository.GetBookingAsync(id.Value);

            if (order == null)
            {
                return NotFound();
            }

            var model = new DepartureViewModel
            {
                Id = order.Id,
                DepartureDate = DateTime.Today
            };

            return View(model);
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

        [HttpPost]
        public async Task<IActionResult> TakeOff(DepartureViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _bookingRepository.DepartureBooking(model);
                return RedirectToAction("Index");
            }

            return View();
        }

        public async Task<IActionResult> Delete(int? id)
        {
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
