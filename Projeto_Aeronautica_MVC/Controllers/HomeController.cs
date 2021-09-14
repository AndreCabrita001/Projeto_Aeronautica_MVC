using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Projeto_Aeronautica_MVC.Data.Entities;
using Projeto_Aeronautica_MVC.Helpers;
using Projeto_Aeronautica_MVC.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Projeto_Aeronautica_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserHelper _userHelper;

        public HomeController(
            ILogger<HomeController> logger,
            IUserHelper userHelper)
        {
            _logger = logger;
            _userHelper = userHelper;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated && TempData["Redirect"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
