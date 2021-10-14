using Microsoft.AspNetCore.Http;
using Projeto_Aeronautica_MVC.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Projeto_Aeronautica_MVC.Models
{
    public class AddClientViewModel : User
    {
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }
    }
}
