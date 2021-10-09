using Projeto_Aeronautica_MVC.Data.Entities;
using Projeto_Aeronautica_MVC.Models;
using System;

namespace Projeto_Aeronautica_MVC.Helpers
{
    public class ConverterHelper : IConverterHelper
    {

        public Flight ToFlight(FlightViewModel model, Guid imageId, bool isNew)
        {
            return new Flight
            {
                Id = isNew ? 0 : model.Id,
                ImageId = imageId,
                AirplaneId = model.AirplaneId,
                IsAvailable = model.IsAvailable,
                FlightOrigin = model.FlightOrigin,
                FlightDestiny = model.FlightDestiny,
                DepartureDate = model.DepartureDate,
                AvaliableSeats = model.AvaliableSeats,
                Price = model.Price,
                User = model.User
            };
        }

        public FlightViewModel ToFlightViewModel(Flight flight)
        {
            return new FlightViewModel
            {
                Id = flight.Id,
                FlightApparatus = flight.FlightApparatus,
                IsAvailable = flight.IsAvailable,
                AirplaneId = flight.AirplaneId,
                FlightOrigin = flight.FlightOrigin,
                FlightDestiny = flight.FlightDestiny,
                DepartureDate = flight.DepartureDate,
                CityOriginId = flight.CityOriginId,
                CityDestinyId = flight.CityDestinyId,
                AvaliableSeats = flight.AvaliableSeats,
                Price = flight.Price,
                User = flight.User
            };
        }

        public Airplane ToPlane(AirplaneViewModel model, Guid imageId, bool isNew)
        {
            return new AirplaneViewModel
            {
                Id = isNew ? 0 : model.Id,
                ImageId = imageId,
                Apparatus = model.Apparatus,
                NumberOfColumns = model.NumberOfColumns,
                TotalSeats = model.TotalSeats,
                SeatsPerColumn = model.SeatsPerColumn,
                AvaliableSeats = model.AvaliableSeats,
                IsAvailable = model.IsAvailable
            };
        }

        public AirplaneViewModel ToPlaneViewModel(Airplane airplane)
        {
            return new AirplaneViewModel
            {
                Id = airplane.Id,
                Apparatus = airplane.Apparatus,
                NumberOfColumns = airplane.NumberOfColumns,
                TotalSeats = airplane.TotalSeats,
                SeatsPerColumn = airplane.SeatsPerColumn,
                AvaliableSeats = airplane.AvaliableSeats,
                IsAvailable = airplane.IsAvailable
            };
        }

        public User ToUser(RegisterNewUserViewModel model, Guid imageId, bool isNew)
        {
            return new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                ImageId = imageId,
                Email = model.Username,
                UserName = model.Username,
                Address = model.Address,
                PhoneNumber = model.PhoneNumber,
                Country = model.Country,
                //CityId = model.CityId,
            };
        }

        public RegisterNewUserViewModel ToUserViewModel(User user)
        {
            return new RegisterNewUserViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.UserName,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                //CityId = user.CityId,
            };
        }

        public User ToChangeUser(RegisterNewUserViewModel model, Guid imageId, bool isNew)
        {
            return new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                ImageId = imageId,
                Address = model.Address,
                PhoneNumber = model.PhoneNumber,
                Country = model.Country
                //CityId = model.CityId
            };
        }

        public RegisterNewUserViewModel ToChangeUserViewModel(User user, Guid imageId)
        {
            return new RegisterNewUserViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                ImageId = imageId,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
            };
        }
    }
}
