using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Projeto_Aeronautica_MVC.Data.Entities;

namespace Projeto_Aeronautica_MVC.Models
{
    public class AddTicketViewModel
    {
        [Display(Name = "Flight")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a flight.")]
        public int FlightId { get; set; }

        //public Flight Flight { get; set; }

        [Range(0.0001, double.MaxValue, ErrorMessage = "The quantity must be a positive number.")]
        public double Quantity { get; set; }

        public IEnumerable<SelectListItem> Flights { get; set; }
    }
}
