﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Projeto_Aeronautica_MVC.Data.Entities
{
    public class Flight : IEntity
    {
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Display(Name = "Adult Price")]
        public decimal AdultPrice { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Display(Name = "Child Price")]
        public decimal ChildPrice { get; set; }

        [Display(Name = "Image")]
        public Guid ImageId { get; set; }

        [Display(Name = "Departure Date")]
        public DateTime? DepartureDate { get; set; }

        [Display(Name = "Arrival Date")]
        public DateTime? ArrivalDate { get; set; }

        [Display(Name = "Flight Origin")]
        public string FlightOrigin { get; set; }

        [Display(Name = "Flight Destiny")]
        public string FlightDestiny { get; set; }

        [Display(Name = "Flight Apparatus")]
        public string FlightApparatus { get; set; }

        [Display(Name = "Is Available")]
        public bool IsAvailable { get; set; }

        public User User { get; set; }

        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://supershoptpsi.azurewebsites.net/images/noimage.png"
            : $"https://supershoptpsi.blob.core.windows.net/products/{ImageId}";
    }
}