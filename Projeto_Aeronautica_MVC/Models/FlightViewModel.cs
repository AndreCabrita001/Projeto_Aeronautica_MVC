using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Projeto_Aeronautica_MVC.Data.Entities;

namespace Projeto_Aeronautica_MVC.Models
{
    public class FlightViewModel : Flight
    {
        [Display(Name ="Image")]
        public IFormFile ImageFile { get; set; }

        //[Display(Name = "Flight")]
        //[Range(1, int.MaxValue, ErrorMessage = "You must select a flight.")]
        //public int FlightId { get; set; }

        //[Display(Name = "Flight Apparatus")]
        //public string ApparatusName { get; set; }

        

        public IEnumerable<SelectListItem> Flights { get; set; }

        public IEnumerable<SelectListItem> FlightsOrigin { get; set; }

        public IEnumerable<SelectListItem> FlightsDestiny { get; set; }
    }
}
