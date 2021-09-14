using Projeto_Aeronautica_MVC.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Projeto_Aeronautica_MVC.Models
{
    public class LoginViewModel : User
    {
        [Required]
        [EmailAddress]
        public string Username { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }


        public bool RememberMe { get; set; }
    }
}
