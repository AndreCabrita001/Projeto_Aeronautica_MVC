using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Projeto_Aeronautica_MVC.Data.Entities
{
    public class Airplane : IEntity
    {
        public int Id { get; set; }

        [Display(Name = "Flight Apparatus")]
        public string Apparatus { get; set; }

        [Display(Name = "Seat Columns")]
        public int NumberOfColumns { get; set; }

        [Display(Name = "Total Seats")]
        public int TotalSeats { get; set; }

        [Display(Name = "Seats per Row")]
        public int SeatsPerColumn { get; set; }

        [Display(Name = "Is Available")]
        public bool IsAvailable { get; set; }

        [Display(Name = "Image")]
        public Guid ImageId { get; set; }

        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://projetoaeronauticamvc123.azurewebsites.net/images/noimage.png"
            : $"https://projaerostoragemvc.blob.core.windows.net/airplanes/{ImageId}";
    }
}
