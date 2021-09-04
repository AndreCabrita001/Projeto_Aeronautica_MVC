using Microsoft.AspNetCore.Identity;

namespace Projeto_Aeronautica_MVC.Data.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }


        public string LastName { get; set; }
    }
}
