using Projeto_Aeronautica_MVC.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projeto_Aeronautica_MVC.Data
{
    public interface IAirplaneRepository: IGenericRepository<Airplane>
    {
        public IQueryable GetAllWithFlights();
    }
}
