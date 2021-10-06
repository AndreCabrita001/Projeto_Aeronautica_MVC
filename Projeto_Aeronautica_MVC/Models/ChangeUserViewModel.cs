using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Projeto_Aeronautica_MVC.Data.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Projeto_Aeronautica_MVC.Models
{
    public class ChangeUserViewModel : User
    {
        //[Required]
        //[Display(Name = "First Name")]
        //public string FirstName { get; set; }

        //[Required]
        //[Display(Name = "Last Name")]
        //public string LastName { get; set; }

        //[MaxLength(100, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        //public string Address { get; set; }

        //[MaxLength(20, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        //public string PhoneNumber { get; set; }

        [Display(Name = "Country")]
        public string SelectedCountry { get; set; }

        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }
    }
}
