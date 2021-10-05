using Microsoft.AspNetCore.Mvc.Rendering;
using Projeto_Aeronautica_MVC.Data.Entities;
using System.Collections.Generic;
using System.Linq;


namespace Projeto_Aeronautica_MVC.Data
{
    public interface IFlightRepository : IGenericRepository<Flight>
    {
        public IQueryable GetAllWithUsers();

        public IEnumerable<SelectListItem> GetComboFlightDestiny();

        public IEnumerable<SelectListItem> GetComboFlightApparatus();
    }
}
