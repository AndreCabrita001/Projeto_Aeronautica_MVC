﻿using System;
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
        private Random _random;

        public SeedDb(DataContext context,IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _random = new Random();
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

            var user = await _userHelper.GetUserByEmailAsync("aandrecaldeira15@gmail.com");
            if(user == null)
            {
                user = new User
                {
                    FirstName = "André",
                    LastName = "Cabrita",
                    Email = "aandrecaldeira15@gmail.com",
                    UserName = "aandrecaldeira15@gmail.com",
                    PhoneNumber = "212343555",
                    Address = "Rua Jau 33",
                    CityId = _context.Countries.FirstOrDefault().Cities.FirstOrDefault().Id,
                    City = _context.Countries.FirstOrDefault().Cities.FirstOrDefault()
                };

                var result = await _userHelper.AddUserAsync(user, "123456");
                if(result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }

                await _userHelper.AddUserToRoleAsync(user, "Admin");
            }

            var isInRole = await _userHelper.IsUserInRoleAsync(user, "Admin");
            if(!isInRole)
            {
                await _userHelper.AddUserToRoleAsync(user, "Admin");
            }

            if (!_context.Flights.Any())
            {
                AddFlight("Airbus 300", user);
                AddFlight("Airbus 301", user);
                AddFlight("Airbus 302", user);
                AddFlight("Airbus 303", user);
                await _context.SaveChangesAsync();
            }
        }

        private void AddFlight(string name, User user)
        {
            _context.Flights.Add(new Flight
            {
                FlightApparatus = name,
                FlightDestiny = "France",
                DepartureDate = DateTime.Now,
                AdultPrice = _random.Next(1000),
                IsAvailable = true,
                User = user
            });
        }
    }
}
