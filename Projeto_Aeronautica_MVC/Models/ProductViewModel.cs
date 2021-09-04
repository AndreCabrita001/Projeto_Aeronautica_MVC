﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Projeto_Aeronautica_MVC.Data.Entities;

namespace Projeto_Aeronautica_MVC.Models
{
    public class ProductViewModel : Product
    {
        [Display(Name ="Image")]
        public IFormFile ImageFile { get; set; }
    }
}
