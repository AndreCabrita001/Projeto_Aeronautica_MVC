using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projeto_Aeronautica_MVC.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projeto_Aeronautica_MVC.Data
{
    public class AirplaneRepository : GenericRepository<Airplane>, IAirplaneRepository
    {
        private readonly DataContext _context;

        public AirplaneRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable GetAllWithFlights()
        {
            return _context.Airplanes.Include(p => p.Apparatus);
        }

        public IEnumerable<SelectListItem> GetComboFlightApparatus()
        {
            var list = _context.Airplanes.Select(p => new SelectListItem
            {
                Text = p.Apparatus,
                Value = p.Id.ToString()
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select an airplane...)",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboColumnLetters()
        {
            List<string> lettersList = new List<string>();

            var ApparatusList = _context.Airplanes.Include(p => p.Apparatus);

            var columnNumberlist = _context.Airplanes.Select(p => new SelectListItem
            {
                Text = p.NumberOfColumns.ToString(),
                Value = p.Apparatus

            }).ToList();

            var columnNumber = 0;
            foreach(var item in columnNumberlist)
            {
                columnNumber = Convert.ToInt32(item.Value);
            }

            var list = _context.Airplanes.Select(p => new SelectListItem
            {
              
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a Column Letter)",
                Value = "0"
            });

            return list;
        }
    }
}
