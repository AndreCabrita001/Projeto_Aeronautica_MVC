using System;
using System.ComponentModel.DataAnnotations;

namespace Projeto_Aeronautica_MVC.Models
{
    public class DepartureViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Departure date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime DepartureDate { get; set; }
    }
}
