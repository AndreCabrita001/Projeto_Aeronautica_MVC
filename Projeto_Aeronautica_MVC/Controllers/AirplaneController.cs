using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projeto_Aeronautica_MVC.Data;
using Projeto_Aeronautica_MVC.Helpers;
using Projeto_Aeronautica_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projeto_Aeronautica_MVC.Controllers
{
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
        [Authorize(Roles="Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: AirplaneController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AirplaneViewModel model)
        {
            return View(model);
        }

        // GET: AirplaneController/Edit/5
        public IActionResult Edit(int id)
        {
            return View();
        }

        // POST: AirplaneController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AirplaneController/Delete/5
        public IActionResult Delete(int id)
        {
            return View();
        }

        // POST: AirplaneController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
