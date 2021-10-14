using System;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto_Aeronautica_MVC.Data;
using Projeto_Aeronautica_MVC.Data.Entities;
using Projeto_Aeronautica_MVC.Helpers;
using Projeto_Aeronautica_MVC.Models;

namespace Projeto_Aeronautica_MVC.Controllers
{
    public class FlightsController : Controller
    {
        private readonly DataContext _context;
        private readonly IFlightRepository _flightRepository;
        private readonly IAirplaneRepository _airplaneRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IUserHelper _userHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IBookingRepository _bookingRepository;

        public FlightsController(
            IFlightRepository flightRepository,
             IBookingRepository bookingRepository,
            IAirplaneRepository airplaneRepository,
            ICountryRepository countryRepository,
            IUserHelper userHelper,
            IBlobHelper blobHelper,
            IConverterHelper converterHelper,
            DataContext context)
        {
            _flightRepository = flightRepository;
            _airplaneRepository = airplaneRepository;
            _countryRepository = countryRepository;
            _userHelper = userHelper;
            _blobHelper = blobHelper;
            _bookingRepository = bookingRepository;
            _converterHelper = converterHelper;
            _context = context;
        }

        // GET: Flights
        public IActionResult Index()
        {
            return View(_flightRepository.GetAll().OrderBy(p => p.FlightApparatus));
        }

        [HttpGet]
        public async Task<IActionResult> Index(string flightSearch)
        {
            ViewData["GetFlightDetails"] = flightSearch;

            var flightQuery = from x in _flightRepository.GetAll() select x;

            if (!String.IsNullOrEmpty(flightSearch))
            {
                flightQuery = flightQuery.Where(x => x.FlightApparatus.Contains(flightSearch) ||
                x.Price.ToString().Contains(flightSearch) || x.FlightOrigin.Contains(flightSearch) ||
                x.FlightDestiny.Contains(flightSearch) || x.DepartureDate.ToString().Contains(flightSearch) ||
                x.AvailableSeats.ToString().Contains(flightSearch) || x.CityOrigin.Contains(flightSearch) ||
                x.CityDestiny.Contains(flightSearch));
            }

            return View(await flightQuery.AsNoTracking().ToListAsync());
        }

        // GET: Flights/Details/5
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
            var model = new FlightViewModel
            {
                Flights = _airplaneRepository.GetComboFlightApparatus(),
                FlightsDestiny = _countryRepository.GetComboCountries(),
                FlightsOrigin = _countryRepository.GetComboCountries(),
                //Cities = _countryRepository.GetComboCities(0),
            };

            return View(model);
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
                if(model.DepartureDate < DateTime.Now)
                {
                    TempData["CreateFlightError"] = "The departure cannot occur in the past.";

                    return RedirectToAction("Create");
                }


                var airplane = await _airplaneRepository.GetByIdAsync(model.AirplaneId);

                if(airplane.IsAvailable == false)
                {
                    TempData["CreateFlightError"] = "This airplane is currently unavaliable";

                    return RedirectToAction("Create");
                }

                Guid imageId = airplane.ImageId;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "airplanes");
                }

                var flight = _converterHelper.ToFlight(model, imageId, true);

                flight.FlightApparatus = airplane.Apparatus;
                //flight.AvaliableSeats = airplane.AvaliableSeats;

                var OriginCity = await _countryRepository.GetCityAsync(model.CityOriginId);
                var DestinyCity = await _countryRepository.GetCityAsync(model.CityDestinyId);

                if(OriginCity == DestinyCity)
                {
                    TempData["CreateFlightError"] = "The Flight Origin cannot be the same as it's Destiny.";

                    return RedirectToAction("Create");
                }

                var flightOrigin = await _countryRepository.GetByIdAsync(model.FlightOriginId);
                var flightDestiny = await _countryRepository.GetByIdAsync(model.FlightDestinyId);

                flight.FlightOrigin = flightOrigin.Name;
                flight.FlightDestiny = flightDestiny.Name;

                flight.CityOrigin = OriginCity.Name;
                flight.CityDestiny = DestinyCity.Name;

                flight.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

                airplane.IsAvailable = false;

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

            Guid imageId = flight.ImageId;

            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "airplanes");
            }

            model.ImageId = imageId;
            model.Flights = _airplaneRepository.GetComboFlightApparatus();
            model.FlightsOrigin = _countryRepository.GetComboCountries();
            model.FlightsDestiny = _countryRepository.GetComboCountries();

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
                if (model.DepartureDate < DateTime.Now)
                {
                    TempData["EditFlightError"] = "The departure cannot occur in the past.";

                    return RedirectToAction("Edit");
                }

                try
                {
                    Guid imageId = model.ImageId;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "airplanes");
                    }
                    var airplane = await _airplaneRepository.GetByIdAsync(model.AirplaneId);

                    bool isNew = false;
                    var flight = new Flight
                    {
                        Id = isNew ? 0 : model.Id,
                        FlightApparatus = airplane.Apparatus,
                        AvailableSeats = model.AvailableSeats,
                        IsAvailable = model.IsAvailable,
                        ImageId = imageId,
                        AirplaneId = model.AirplaneId,
                        FlightOrigin = model.FlightOrigin,
                        FlightDestiny = model.FlightDestiny,
                        DepartureDate = model.DepartureDate,
                        Price = model.Price,
                        User = model.User
                    };

                    var OriginCity = await _countryRepository.GetCityAsync(model.CityOriginId);
                    var DestinyCity = await _countryRepository.GetCityAsync(model.CityDestinyId);

                    if (OriginCity == DestinyCity)
                    {
                        TempData["EditFlightError"] = "The Flight Origin cannot be the same as it's Destiny.";

                        return RedirectToAction("Edit");
                    }

                    var flightOrigin = await _countryRepository.GetByIdAsync(model.FlightOriginId);
                    var flightDestiny = await _countryRepository.GetByIdAsync(model.FlightDestinyId);

                    flight.FlightOrigin = flightOrigin.Name;
                    flight.FlightDestiny = flightDestiny.Name;

                    flight.CityOrigin = OriginCity.Name;
                    flight.CityDestiny = DestinyCity.Name;

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

            var airplane = await _airplaneRepository.GetByIdAsync(flight.AirplaneId);

            var bookings = _context.Bookings.Where(o => o.FlightId == flight.Id);
            try
            {
                await _flightRepository.DeleteAsync(flight);
                
                foreach(var item in bookings)
                {
                    _context.Remove(item);
                }

                await _context.SaveChangesAsync();
                airplane.IsAvailable = true;
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ViewBag.ErrorTitle = $"This flight is currently in use.";
                ViewBag.ErrorMessage = $"Unable to delete this flight.<br>" +
                    $"Try to remove it's usage components...";
            }

            return View("Error");
        }

        [HttpPost]
        [Route("Flights/GetCitiesAsync")]
        public async Task<JsonResult> GetCitiesAsync(int countryId)
        {
            var country = await _countryRepository.GetCountryWithCitiesAsync(countryId);

            var json = country.Cities.OrderBy(c => c.Name);

            return Json(json);
        }

    }
}
