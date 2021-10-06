using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Projeto_Aeronautica_MVC.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Projeto_Aeronautica_MVC.Models
{ 
    public class RegisterNewUserViewModel : User
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }

        //public IEnumerable<SelectListItem> Cities { get; set; }

        //[Display(Name = "Country")]
        //[Range(1, int.MaxValue, ErrorMessage = "You must select a country.")]
        //public int CountryId { get; set; }

        [Display(Name = "Country")]
        public string SelectedCountry { get; set; }

        //public IEnumerable<SelectListItem> Countries { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string Confirm { get; set; }

        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }
    }
}
