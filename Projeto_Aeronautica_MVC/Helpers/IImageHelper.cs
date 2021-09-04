using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace Projeto_Aeronautica_MVC.Helpers
{
    public interface IImageHelper
    {
        Task<string> UploadImageAsync(IFormFile imageFile, string folder);
    }
}
