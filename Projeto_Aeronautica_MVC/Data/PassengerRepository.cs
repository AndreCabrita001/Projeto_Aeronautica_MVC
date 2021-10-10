using Microsoft.EntityFrameworkCore;
using Projeto_Aeronautica_MVC.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projeto_Aeronautica_MVC.Data
{
    public class PassengerRepository :  IPassengerRepository
    {

        private readonly DataContext _context;

        public PassengerRepository(DataContext context)
        {
            _context = context;
        }

        public Task CreateAsync(Passenger entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Passenger entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistAsync(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Passenger> GetAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Passenger> GetAllPassengers()
        {
            return _context.Set<Passenger>();
        }

        public Task<Passenger> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Passenger entity)
        {
            throw new NotImplementedException();
        }
    }
}
