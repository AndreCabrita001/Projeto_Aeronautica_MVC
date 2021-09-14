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
                FlightApparatus = model.FlightApparatus,
                IsAvailable = model.IsAvailable,
                FlightOrigin = model.FlightOrigin,
                FlightDestiny = model.FlightDestiny,
                DepartureDate = model.DepartureDate,
                ArrivalDate = model.ArrivalDate,
                AdultPrice = model.AdultPrice,
                //ChildPrice = model.ChildPrice,
                User = model.User
            };
        }

        public FlightViewModel ToFlightViewModel(Flight flight)
        {
            return new FlightViewModel
            {
                Id = flight.Id,
                ImageId = flight.ImageId,
                FlightApparatus = flight.FlightApparatus,
                IsAvailable = flight.IsAvailable,
                FlightOrigin = flight.FlightOrigin,
                FlightDestiny = flight.FlightDestiny,
                DepartureDate = flight.DepartureDate,
                ArrivalDate = flight.ArrivalDate,
                AdultPrice = flight.AdultPrice,
                //ChildPrice = flight.ChildPrice,
                User = flight.User
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
                CityId = model.CityId,
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
                CityId = user.CityId,
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
                CityId = model.CityId
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
