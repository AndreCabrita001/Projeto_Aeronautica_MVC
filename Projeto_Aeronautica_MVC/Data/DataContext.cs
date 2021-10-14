using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Projeto_Aeronautica_MVC.Data.Entities;

namespace Projeto_Aeronautica_MVC.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DbSet<Flight> Flights { get; set; }

        public DbSet<Airplane> Airplanes { get; set; }

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<BookingDetail> BookingDetails { get; set; }

        public DbSet <BookingDetailTemp> BookingDetailsTemp { get; set; }

        public DbSet<BookingHistory> BookingsHistory { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }


    }
}
