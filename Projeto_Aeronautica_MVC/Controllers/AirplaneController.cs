using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto_Aeronautica_MVC.Data;
using Projeto_Aeronautica_MVC.Data.Entities;
using Projeto_Aeronautica_MVC.Helpers;
using Projeto_Aeronautica_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projeto_Aeronautica_MVC.Controllers
{
    [Authorize (Roles ="Admin")]
    public class AirplaneController : Controller
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IAirplaneRepository _airplaneRepository;
        private readonly IUserHelper _userHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;
        public AirplaneController(
            IFlightRepository flightRepository,
            IAirplaneRepository airplaneRepository,
            IUserHelper userHelper,
            IBlobHelper blobHelper,
            IConverterHelper converterHelper)
        {
            _flightRepository = flightRepository;
            _airplaneRepository = airplaneRepository;
            _userHelper = userHelper;
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
        }

        // GET: AirplaneController
        public IActionResult Index()
        {
            return View(_airplaneRepository.GetAll().OrderBy(p => p.Apparatus));
        }

        // GET: AirplaneController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airplane = await _airplaneRepository.GetByIdAsync(id.Value);

            if (airplane == null)
            {
                return NotFound();
            }

            return View(airplane);
        }

        // GET: AirplaneController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AirplaneController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AirplaneViewModel model)
        {
            if (ModelState.IsValid)
            {
                if(model.AvaliableSeats > model.TotalSeats)
                {
                    TempData["AddAirplaneError"] = "The number of Avaliable Seats cannot be higher than the" +
                        "Total Seats";

                    return RedirectToAction("Create");
                }

                if(model.NumberOfColumns > 9)
                {
                    TempData["AddAirplaneError"] = "The maximum number of Columns for an airplane is 9";

                    return RedirectToAction("Create");
                }



                Guid imageId = Guid.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "airplanes");
                }

                var airplane = new Airplane()
                {
                    Id = model.Id,
                    ImageId = imageId,
                    Apparatus = model.Apparatus,
                    NumberOfColumns = model.NumberOfColumns,
                    TotalSeats = model.TotalSeats,
                    AvaliableSeats = model.AvaliableSeats,
                    IsAvailable = model.IsAvailable
                };

                airplane.SeatsPerColumn = airplane.TotalSeats / airplane.NumberOfColumns;

                await _airplaneRepository.CreateAsync(airplane);

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: AirplaneController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airplane = await _airplaneRepository.GetByIdAsync(id.Value);
            if (airplane == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToPlaneViewModel(airplane);

            Guid imageId = airplane.ImageId;

            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "airplanes");
            }

            model.ImageId = imageId;

            return View(model);
        }

        // POST: AirplaneController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AirplaneViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Guid imageId = model.ImageId;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "airplanes");
                    }

                    var airplane = _converterHelper.ToPlane(model, imageId, false);

                    airplane.SeatsPerColumn = airplane.TotalSeats / airplane.NumberOfColumns;

                    await _airplaneRepository.UpdateAsync(airplane);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _airplaneRepository.ExistAsync(model.Id))
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

        // GET: AirplaneController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //dynamic FlightAirplane = new ExpandoObject();

            var airplane = await _airplaneRepository.GetByIdAsync(id.Value);

            if (airplane == null)
            {
                return NotFound();
            }

            return View(airplane);
        }

        // POST: AirplaneController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var airplane = await _airplaneRepository.GetByIdAsync(id);

            try
            {
                await _airplaneRepository.DeleteAsync(airplane);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ViewBag.ErrorTitle = $"This airplane is currently in use.";
                ViewBag.ErrorMessage = $"Unable to delete this airplane.<br>" +
                    $"Try to remove it's usage components...";
            }

            return View("Error");
        }
    }
}
