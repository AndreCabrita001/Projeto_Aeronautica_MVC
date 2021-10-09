using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Projeto_Aeronautica_MVC.Data.Entities
{
    public class Flight : IEntity
    {
        public int Id { get; set; }


        [Range(1, int.MaxValue, ErrorMessage = "You must select an airplane.")]
        public int AirplaneId { get; set; }

        public Airplane Airplane { get; set; }

        [Display(Name = "Flight Apparatus")]
        public string FlightApparatus { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Display(Name = "Price p/Ticket")]
        public decimal Price { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        [Display(Name = "Departure Date")]
        public DateTime? DepartureDate { get; set; }

        //[Required]
        //[DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        //[Display(Name = "Arrival Date")]
        //public DateTime? ArrivalDate { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "You must select a Flight Origin.")]
        public int FlightOriginId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "You must select a City Origin.")]
        public int CityOriginId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "You must select a Flight Destiny.")]
        public int FlightDestinyId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "You must select a City Destiny.")]
        public int CityDestinyId { get; set; }

        [Display(Name = "Flight Origin")]
        public string FlightOrigin { get; set; }

        [Display(Name = "City Origin")]
        public string CityOrigin { get; set; }

        [Display(Name = "Flight Destiny")]
        public string FlightDestiny { get; set; }

        [Display(Name = "City Origin")]
        public string CityDestiny { get; set; }

        [Display(Name = "Avaliable Seats")]
        public int AvaliableSeats { get; set; }

        [Display(Name = "Is Available")]
        public bool IsAvailable { get; set; }

        [Display(Name = "Image")]
        public Guid ImageId { get; set; }
        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://projetoaeronauticamvc.azurewebsites.net/images/noimage.png"
            : $"https://projaerostoragemvc.blob.core.windows.net/airplanes/{ImageId}";
        public User User { get; set; }
    }
}