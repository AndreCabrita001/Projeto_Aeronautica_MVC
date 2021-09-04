using Projeto_Aeronautica_MVC.Data.Entities;
using Projeto_Aeronautica_MVC.Models;
using System;


namespace Projeto_Aeronautica_MVC.Helpers
{
    public interface IConverterHelper
    {
        Product ToProduct(ProductViewModel model, bool isNew);

        ProductViewModel ToProductViewModel(Product product);
    }
}
