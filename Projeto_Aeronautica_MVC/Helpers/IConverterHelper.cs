using Projeto_Aeronautica_MVC.Data.Entities;
using Projeto_Aeronautica_MVC.Models;
using System;


namespace Projeto_Aeronautica_MVC.Helpers
{
    public interface IConverterHelper
    {
        Flight ToFlight(FlightViewModel model, Guid imageId, bool isNew);

        FlightViewModel ToFlightViewModel(Flight flight);
    }
}
