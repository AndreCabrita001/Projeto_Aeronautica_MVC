using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projeto_Aeronautica_MVC.Data;
using Projeto_Aeronautica_MVC.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Projeto_Aeronautica_MVC.Data
{
    public class FlightRepository : GenericRepository<Flight>, IFlightRepository
    {
        private readonly DataContext _context;

        public FlightRepository(DataContext context) : base(context)
        {
            _context = context;
        }


        public IQueryable GetAllWithUsers()
        {
            return _context.Flights.Include(p => p.User);
        }

        public IEnumerable<SelectListItem> GetComboFlightDestiny()
        {
            var list = _context.Flights.Select(p => new SelectListItem
            {
                Text = p.FlightDestiny,
                Value = p.Id.ToString()
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a flight...)",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboFlightApparatus()
        {
            var list = _context.Flights.Select(p => new SelectListItem
            {
                Text = p.FlightApparatus,
                Value = p.Id.ToString()

            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select an airplane...)",
                Value = "0"
            });

            return list;
        }
    }
}
