using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Projeto_Aeronautica_MVC.Data.Entities;

namespace Projeto_Aeronautica_MVC.Models
{
    public class AddTicketViewModel
    {
        [Display(Name = "Column")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a flight.")]
        public int FlightId { get; set; }

        [Display(Name = "Column")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a seat column.")]
        public int ColumnId { get; set; }

        [Display(Name = "Column")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a seat column number.")]
        public int ColumnNumberId { get; set; }

        [Range(0.0001, double.MaxValue, ErrorMessage = "The quantity must be a positive number.")]
        public double Quantity { get; set; }

        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string FirstName { get; set; }

        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string LastName { get; set; }

        [MaxLength(100, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string Address { get; set; }

        [Display(Name = "Nationality")]
        public string Nationality { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Seat Name")]
        public string SeatName { get; set; }

        public IEnumerable<SelectListItem> Flights { get; set; }
    }
}
