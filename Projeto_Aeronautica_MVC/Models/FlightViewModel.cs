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

        public IEnumerable<SelectListItem> Cities { get; set; }

        public IEnumerable<SelectListItem> Flights { get; set; }

        public IEnumerable<SelectListItem> FlightsOrigin { get; set; }

        public IEnumerable<SelectListItem> FlightsDestiny { get; set; }
    }
}
