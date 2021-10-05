using Projeto_Aeronautica_MVC.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Projeto_Aeronautica_MVC.Data.Entities
{
    public class BookingDetail : IEntity
    {
        public int Id { get; set; }

        [Required]
        public Flight Flight { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Price { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double Quantity { get; set; }

        public decimal Value => Price * (decimal)Quantity;
    }
}
