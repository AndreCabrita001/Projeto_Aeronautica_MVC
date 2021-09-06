using Projeto_Aeronautica_MVC.Data.Entities;
using System.Linq;


namespace Projeto_Aeronautica_MVC.Data
{
    public interface IFlightRepository : IGenericRepository<Flight>
    {
        public IQueryable GetAllWithUsers();
    }
}
