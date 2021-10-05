using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace Projeto_Aeronautica_MVC.Data.Entities
{
    public class User : IdentityUser, IEntity
    {
        int IEntity.Id { get; set; }

        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string FirstName { get; set; }

        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string LastName { get; set; }

        [MaxLength(100,ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string Address { get; set; }

        public string Country { get; set; }

        //public int CityId { get; set; }

        //public City City { get; set; }

        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";

        [Display(Name = "Image")]
        public Guid ImageId { get; set; }

        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://projetoaeronautica.azurewebsites.net/images/noimage.png"
            : $"https://projetoaerostorage.blob.core.windows.net/users/{ImageId}";
    }
}
