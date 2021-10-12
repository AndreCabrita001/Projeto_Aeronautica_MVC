using Projeto_Aeronautica_MVC.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Projeto_Aeronautica_MVC.Data.Entities
{
    public class BookingDetail : IEntity
    {
        public int Id { get; set; }

        public int BookingId { get; set; }
        
        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string FirstName { get; set; }

        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string LastName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";

        [MaxLength(100, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string Address { get; set; }

        [Display(Name = "Nationality")]
        public string Nationality { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Flight Destiny")]
        public string FlightDestiny { get; set; }

        [Display(Name = "City Destiny")]
        public string CityDestiny { get; set; }

        [Display(Name = "Seats Name")]
        public string SeatName { get; set; }

        [Display(Name = "Seats per Column")]
        public int SeatsPerColumn { get; set; }

        public List<BookingDetailTemp> Bookings { get; set; }

        [Display(Name = "Departure date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime? DepartureDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Price { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double Quantity { get; set; }

        public int Value { get; set; }

        public int FlightId { get; set; }

        public Flight Flight { get; set; }
    }
}
