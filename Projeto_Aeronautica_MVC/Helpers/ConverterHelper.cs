using Projeto_Aeronautica_MVC.Data.Entities;
using Projeto_Aeronautica_MVC.Models;
using System;

namespace Projeto_Aeronautica_MVC.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public Flight ToFlight(FlightViewModel model, string path, bool isNew)
        {
            return new Flight
            {
                Id = isNew ? 0 : model.Id,
                ImageUrl = path,
                FlightApparatus = model.FlightApparatus,
                IsAvailable = model.IsAvailable,
                FlightOrigin = model.FlightOrigin,
                FlightDestiny = model.FlightDestiny,
                DepartureDate = model.DepartureDate,
                ArrivalDate = model.ArrivalDate,
                AdultPrice = model.AdultPrice,
                ChildPrice = model.ChildPrice,
                User = model.User
            };
        }

        public FlightViewModel ToFlightViewModel(Flight flight)
        {
            return new FlightViewModel
            {
                Id = flight.Id,
                ImageUrl = flight.ImageUrl,
                FlightApparatus = flight.FlightApparatus,
                IsAvailable = flight.IsAvailable,
                FlightOrigin = flight.FlightOrigin,
                FlightDestiny = flight.FlightDestiny,
                DepartureDate = flight.DepartureDate,
                ArrivalDate = flight.ArrivalDate,
                AdultPrice = flight.AdultPrice,
                ChildPrice = flight.ChildPrice,
                User = flight.User
            };
        }
    }
}
