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
    }
}
