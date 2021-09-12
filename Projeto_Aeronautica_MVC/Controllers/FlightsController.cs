using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto_Aeronautica_MVC.Data;
using Projeto_Aeronautica_MVC.Helpers;
using Projeto_Aeronautica_MVC.Models;

namespace Projeto_Aeronautica_MVC.Controllers
{
   
    public class FlightsController : Controller
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IUserHelper _userHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;

        public FlightsController(
            IFlightRepository flightRepository,
            IUserHelper userHelper,
            IBlobHelper blobHelper,
            IConverterHelper converterHelper)
        {
            _flightRepository = flightRepository;
            _userHelper = userHelper;
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
        }

        // GET: Products
        public IActionResult Index()
        {
            return View(_flightRepository.GetAll().OrderBy(p => p.FlightApparatus));
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _flightRepository.GetByIdAsync(id.Value);
            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }


        // GET: Flights/Create
        [Authorize(Roles = "Employee")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Flights/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FlightViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {

                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "flights");
                }


                var flight = _converterHelper.ToFlight(model, imageId, true);

                flight.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                await _flightRepository.CreateAsync(flight);

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }



        // GET: Flights/Edit/5
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _flightRepository.GetByIdAsync(id.Value);
            if (flight == null)
            {
                return NotFound();
            }


            var model = _converterHelper.ToFlightViewModel(flight);
            return View(model);
        }


        // POST: Flights/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(FlightViewModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    Guid imageId = model.ImageId;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "flights");
                    }

                    var flight = _converterHelper.ToFlight(model, imageId, false);

                    flight.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                    await _flightRepository.UpdateAsync(flight);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _flightRepository.ExistAsync(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Flights/Delete/5
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _flightRepository.GetByIdAsync(id.Value);
            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }

        // POST: Flights/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var flight = await _flightRepository.GetByIdAsync(id);
            await _flightRepository.DeleteAsync(flight);
            return RedirectToAction(nameof(Index));
        }

    }
}
