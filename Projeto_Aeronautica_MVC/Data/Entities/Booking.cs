using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Projeto_Aeronautica_MVC.Data.Entities
{
    public class Booking : IEntity
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Order date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime BookingDate { get; set; }

        [Display(Name = "Seats per Column")]
        public int SeatsPerColumn { get; set; }

        [Display(Name = "Departure date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime? DepartureDate { get; set; }

        [Required]
        public User User { get; set; }

        public IEnumerable<BookingDetail> Tickets { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int Lines => Tickets == null ? 0 : Tickets.Count();

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double Quantity => Tickets == null ? 0 : Tickets.Sum(i => i.Quantity);

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Value => Tickets == null ? 0 : Tickets.Sum(i => i.Value);

        [Display(Name = "Order date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy HH:mm}", ApplyFormatInEditMode = false)]
        public DateTime? BookingDateLocal =>
#pragma warning disable CS8073 
            this.BookingDate == null
#pragma warning restore CS8073
            ? null
            : this.BookingDate.ToLocalTime();
    }
}
