using Projeto_Aeronautica_MVC.Data.Entities;
using Projeto_Aeronautica_MVC.Models;
using System;


namespace Projeto_Aeronautica_MVC.Helpers
{
    public interface IConverterHelper
    {
        Flight ToFlight(FlightViewModel model, Guid imageId, bool isNew);

        FlightViewModel ToFlightViewModel(Flight flight);


        Airplane ToPlane(AirplaneViewModel model, Guid imageId, bool isNew);

        AirplaneViewModel ToPlaneViewModel(Airplane airplane);


        User ToUser(RegisterNewUserViewModel model, Guid imageId, bool isNew);

        RegisterNewUserViewModel ToUserViewModel(User user);


        User ToChangeUser(RegisterNewUserViewModel model, Guid imageId, bool isNew);

        RegisterNewUserViewModel ToChangeUserViewModel(User user, Guid imageId);

    }
}
