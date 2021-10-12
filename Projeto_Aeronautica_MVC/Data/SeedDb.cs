using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Projeto_Aeronautica_MVC.Data.Entities;
using Projeto_Aeronautica_MVC.Helpers;

namespace Projeto_Aeronautica_MVC.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context,IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            await _userHelper.CheckRoleAsync("Admin");
            await _userHelper.CheckRoleAsync("Employee");
            await _userHelper.CheckRoleAsync("Customer");

            if (!_context.Countries.Any())
            {
                var cities = new List<City>();
                cities.Add(new City { Name = "Lisboa" });
                cities.Add(new City { Name = "Porto" });
                cities.Add(new City { Name = "Faro" });

                _context.Countries.Add(new Country
                {
                    Cities = cities,
                    Name = "Portugal"
                });

                await _context.SaveChangesAsync();
            }

            if (!_context.Countries.Any())
            {
                var cities = new List<City>();
                cities.Add(new City { Name = "Madrid" });
                cities.Add(new City { Name = "Barcelona" });
                cities.Add(new City { Name = "Sevilla" });

                _context.Countries.Add(new Country
                {
                    Cities = cities,
                    Name = "Spain"
                });

                await _context.SaveChangesAsync();
            }


            var user = await _userHelper.GetUserByEmailAsync("aandrecaldeira15@gmail.com");
            var user2 = await _userHelper.GetUserByEmailAsync("aandrecaldeira30@gmail.com");
            if (user == null && user2 == null)
            {
                user = new User
                {
                    FirstName = "André",
                    LastName = "Cabrita",
                    Email = "aandrecaldeira15@gmail.com",
                    UserName = "aandrecaldeira15@gmail.com",
                    PhoneNumber = "212343555",
                    Address = "Rua Jau 33",
                    Country = "Portugal",
                    
                };

                user2 = new User
                {
                    FirstName = "André2",
                    LastName = "Cabrita2",
                    Email = "aandrecaldeira30@gmail.com",
                    UserName = "aandrecaldeira30@gmail.com",
                    PhoneNumber = "424686000",
                    Address = "Rua Jau 66",
                    Country = "Portugal"
                };

                var result = await _userHelper.AddUserAsync(user, "123456");
                var result2 = await _userHelper.AddUserAsync(user2, "123456");

                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }

                if (result2 != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user2 in seeder");
                }

                await _userHelper.AddUserToRoleAsync(user, "Admin");
                await _userHelper.AddUserToRoleAsync(user2, "Employee");

                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                var token2 = await _userHelper.GenerateEmailConfirmationTokenAsync(user2);

                await _userHelper.ConfirmEmailAsync(user, token);
                await _userHelper.ConfirmEmailAsync(user2, token2);
            }

            

            var isInRole = await _userHelper.IsUserInRoleAsync(user, "Admin");
            var isInRole2 = await _userHelper.IsUserInRoleAsync(user2, "Employee");

            if (!isInRole)
            {
                await _userHelper.AddUserToRoleAsync(user, "Admin");
            }

            if (!isInRole2)
            {
                await _userHelper.AddUserToRoleAsync(user2, "Employee");
            }

            if (!_context.Airplanes.Any())
            {
                var plane = new Airplane();

                AddPlane(plane);

                await _context.SaveChangesAsync();

                if (!_context.Flights.Any())
                {
                    AddFlight(plane, user);

                    await _context.SaveChangesAsync();
                }
            }
        }

        private void AddPlane(Airplane plane)
        {
            plane.IsAvailable = true;
            plane.Apparatus = "Airbus a330";
            plane.NumberOfColumns = 3;
            plane.ImageId = new Guid("f62027f2-3f94-4a01-980f-88da906190c1");
            plane.TotalSeats = 300;
            plane.AvaliableSeats = 300;
            plane.SeatsPerColumn = 100;
            _context.Airplanes.Add(plane);
        }

        private void AddFlight(Airplane plane, User user)
        {
            _context.Flights.Add(new Flight
            {
                IsAvailable = true,
                FlightApparatus = plane.Apparatus,
                FlightOrigin = "Portugal",
                FlightDestiny = "Spain",
                ImageId = plane.ImageId,
                DepartureDate = DateTime.Now.AddDays(20),
                Price = 300,
                AvaliableSeats = plane.AvaliableSeats,
                Airplane = plane,
                CityOrigin = "Lisboa",
                CityDestiny = "Madrid",
                User = user
            });
        }
    }
}
