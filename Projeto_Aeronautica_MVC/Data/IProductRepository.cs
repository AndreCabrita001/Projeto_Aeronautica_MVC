using Projeto_Aeronautica_MVC.Data.Entities;
using System.Linq;


namespace Projeto_Aeronautica_MVC.Data
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        public IQueryable GetAllWithUsers();
    }
}
