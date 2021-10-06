using System;
using System.ComponentModel.DataAnnotations;

namespace Projeto_Aeronautica_MVC.Data.Entities
{
    public class Flight : IEntity
    {
        public int Id { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = false)]
        [Display(Name = "Departure Date")]
        public DateTime? DepartureDate { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = false)]
        [Display(Name = "Arrival Date")]
        public DateTime? ArrivalDate { get; set; }

        public int FlightOriginId { get; set; }

        public int FlightDestinyId { get; set; }

        [Display(Name = "Flight Origin")]
        public string FlightOrigin { get; set; }

        [Display(Name = "Flight Destiny")]
        public string FlightDestiny { get; set; }

        [Display(Name = "Flight Apparatus")]
        public string FlightApparatus { get; set; }

        [Display(Name = "Avaliable Seats")]
        public int AvaliableSeats { get; set; }

        [Display(Name = "Is Available")]
        public bool IsAvailable { get; set; }

        [Required]
        public int AirplaneId { get; set; }

        public Airplane Airplane { get; set; }

        [Display(Name = "Image")]
        public Guid ImageId { get; set; }
        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://projetoaeronautica.azurewebsites.net/images/noimage.png"
            : $"https://projetoaerostorage.blob.core.windows.net/airplanes/{ImageId}";
        public User User { get; set; }
    }
}