using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto_Aeronautica_MVC.Data;
using Projeto_Aeronautica_MVC.Models;

namespace Projeto_Aeronautica_MVC.Controllers
{
    public class BookingsController : Controller
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IFlightRepository _flightRepository;

        public BookingsController(IBookingRepository bookingRepository, IFlightRepository flightRepository)
        {

            _bookingRepository = bookingRepository;
            _flightRepository = flightRepository;
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
            var model = new AddTicketViewModel
            {
                Quantity = 1,
                Flights = _flightRepository.GetComboFlightDestiny()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddFlight(AddTicketViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _bookingRepository.AddTicketToBookingAsync(model, this.User.Identity.Name);
                return RedirectToAction("Create");
            }

            return View(model);
        }

        public async Task<IActionResult> DeleteItem(int? id)
        {
            if(id == null)
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

            await _bookingRepository.ModifyBookingDetailTempQuantityAsync(id.Value,1);
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
            if(id == null)
            {
                return NotFound();
            }

            var order = await _bookingRepository.GetBookingAsync(id.Value);

            if(order == null)
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
