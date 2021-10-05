using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Projeto_Aeronautica_MVC.Data;
using Projeto_Aeronautica_MVC.Data.Entities;
using Projeto_Aeronautica_MVC.Helpers;
using Projeto_Aeronautica_MVC.Models;

namespace Projeto_Aeronautica_MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly IMailHelper _mailHelper;
        private readonly IConfiguration _configuration;
        private readonly ICountryRepository _countryRepository;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;
        //private readonly IUserRepository _userRepository;

        public AccountController(
            IUserHelper userHelper,
            IMailHelper mailHelper,
            IConfiguration configuration,
            ICountryRepository countryRepository,
            IBlobHelper blobHelper,
            IConverterHelper converterHelper
            /*IUserRepository userRepository*/)
        {
            _userHelper = userHelper;
            _mailHelper = mailHelper;
            _blobHelper = blobHelper;
            _configuration = configuration;
            _countryRepository = countryRepository;
            _converterHelper = converterHelper;
            //_userRepository = userRepository;
        }

        public async Task<IActionResult> Login()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

                TempData["Redirect"] = "Redirected";
                if (user != null)
                {
                    if (user.ImageId != Guid.Empty)
                    {
                        var imgId = user.ImageId.ToString();
                        TempData["ImageId"] = "https://projetoaerostorage.blob.core.windows.net/users/" + $"{imgId}";
                    }
                    else
                    {
                        TempData["ImageId"] = "https://projetoaeronautica.azurewebsites.net/images/noimage.png";
                    }
                }
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.UserName);

                var result = await _userHelper.LoginAsync(model);
                
                if (result.Succeeded)
                {

                    if (user.ImageId != Guid.Empty)
                    {
                        var imgId = user.ImageId.ToString();
                        TempData["ImageId"] = "https://projetoaerostorage.blob.core.windows.net/users/" + $"{imgId}";
                    }
                    else
                    {
                        TempData["ImageId"] = "https://projetoaeronautica.azurewebsites.net/images/noimage.png";
                    }

                    if (this.Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(this.Request.Query["ReturnUrl"].First());
                    }

                    return this.RedirectToAction("Index", "Home");
                }
            }

            this.ModelState.AddModelError(string.Empty, "Failed to login!");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            var model = new RegisterNewUserViewModel
            {
                //Countries = _countryRepository.GetComboCountries(),
                //Cities = _countryRepository.GetComboCities(0),
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterNewUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Username);
                if (user == null)
                {
                    //var city = await _countryRepository.GetCityAsync(model.CityId);

                    Guid imageId = Guid.Empty;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                    }

                    user = _converterHelper.ToUser(model, imageId, true);
                    //user.City = city;

                    if (this.User.Identity.Name != null && this.User.IsInRole("Admin"))
                    {
                        var result = await _userHelper.AddUserAsync(user, model.Password);
                        await _userHelper.AddUserToRoleAsync(user, "Employee");

                        string myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);
                        //await _userHelper.ConfirmEmailAsync(user, myToken);

                        string tokenLink = Url.Action("ResetPassword", "Account", new
                        {
                            token = myToken
                        }, protocol: HttpContext.Request.Scheme);

                        Response response = _mailHelper.SendEmail(model.Username, "Password Change", $"<h1>Email Confirmation</h1>" +
                            $"Your account has been created by an admin. You are now required to change your password." +
                            $"Please click this link to do so:</br></br><a href = \"{tokenLink}\">Change password</a>");


                        if (response.IsSuccess)
                        {
                            ViewBag.Message = "The authentication instructions have been sent to the employee's email.";
                            return View(model);
                        }
                    }
                    var result2 = await _userHelper.AddUserAsync(user, model.Password);
                    if (result2 != IdentityResult.Success)
                    {
                        ModelState.AddModelError(string.Empty, "The user couldn't be created.");
                        return View(model);
                    }

                    string myToken2 = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                    string tokenLink2 = Url.Action("ConfirmEmail", "Account", new
                    {
                        userid = user.Id,
                        token = myToken2
                    }, protocol: HttpContext.Request.Scheme);

                    Response response2 = _mailHelper.SendEmail(model.Username, "Email confirmation", $"<h1>Email Confirmation</h1>" +
                        $"To activate your account, " +
                        $"please click this link:</br></br><a href = \"{tokenLink2}\">Confirm Email</a>");

                    if (response2.IsSuccess)
                    {
                        ViewBag.Message = "The instructions to activate this account have been sent to your email";
                        return View(model);
                    }

                    ModelState.AddModelError(string.Empty, "The user couldn't be logged.");
                }
            }

            return View(model);
        }

        public async Task<IActionResult> ChangeUser()
        {
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
            var model = new ChangeUserViewModel();
            if (user != null)
            {
                Guid imageId = user.ImageId;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                }

                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.ImageId = imageId;
                model.Address = user.Address;
                model.PhoneNumber = user.PhoneNumber;

                //var city = await _countryRepository.GetCityAsync(user.CityId);
                //if (city != null)
                //{
                //    var country = await _countryRepository.GetCountryAsync(city);
                //    if (country != null)
                //    {
                //        model.CountryId = country.Id;
                //        model.Cities = _countryRepository.GetComboCities(country.Id);
                //        model.Countries = _countryRepository.GetComboCountries();
                //     }
                //}
            }

            model.Cities = _countryRepository.GetComboCities(model.CountryId);
            model.Countries = _countryRepository.GetComboCountries();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUser(ChangeUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                if (user != null)
                {
                    var city = await _countryRepository.GetCityAsync(model.CityId);

                    Guid imageId = user.ImageId;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                    }

                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.ImageId = imageId;
                    user.Address = model.Address;
                    user.PhoneNumber = model.PhoneNumber;
                    //user.CityId = model.CityId;
                    //user.City = city;
                    //user.City = city;

                    if (user.ImageId != Guid.Empty)
                    {
                        var imgId = user.ImageId.ToString();
                        TempData["ImageId"] = "https://projetoaerostorage.blob.core.windows.net/users/" + $"{imgId}";
                    }
                    else
                    {
                        TempData["ImageId"] = "https://projetoaeronautica.azurewebsites.net/images/noimage.png";
                    }

                    var response = await _userHelper.UpdateUserAsync(user);
                    if (response.Succeeded)
                    {
                        ViewBag.UserMessage = "User updated!";
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, response.Errors.FirstOrDefault().Description);
                    }
                }
            }

            return View(model);
        }

        public IActionResult ChangePassword()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                if (user != null)
                {
                    var result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return this.RedirectToAction("ChangeUser");
                    }
                    else
                    {
                        this.ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "User not found.");
                }
            }

            return this.View(model);
        }


        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Username);
                if (user != null)
                {
                    var result = await _userHelper.ValidatePasswordAsync(
                        user,
                        model.Password);

                    if (result.Succeeded)
                    {
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
                        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            _configuration["Tokens:Issuer"],
                            _configuration["Tokens:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddDays(15),
                            signingCredentials: credentials);
                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        };

                        return this.Created(string.Empty, results);

                    }
                }
            }

            return BadRequest();
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return NotFound();
            }

            var user = await _userHelper.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userHelper.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {

            }

            return View();

        }


        public IActionResult RecoverPassword()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "The email doesn't correspont to a registered user.");
                    return View(model);
                }

                var myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);

                var link = this.Url.Action(
                    "ResetPassword",
                    "Account",
                    new { token = myToken }, protocol: HttpContext.Request.Scheme);

                Response response = _mailHelper.SendEmail(model.Email, "Shop Password Reset", $"<h1>Shop Password Reset</h1>" +
                $"To reset the password click in this link:</br></br>" +
                $"<a href = \"{link}\">Reset Password</a>");

                if (response.IsSuccess)
                {
                    this.ViewBag.Message = "The instructions to recover your password have been sent to your email.";
                }

                return this.View();

            }

            return this.View(model);
        }

        public IActionResult ResetPassword(string token)
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await _userHelper.GetUserByEmailAsync(model.UserName);

            await _userHelper.LogoutAsync();

            if (user != null)
            {
                var result = await _userHelper.ResetPasswordAsync(user, model.Token, model.Password);
                if (result.Succeeded)
                {
                    this.ViewBag.Message = "Password reset successfully.";

                    string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                    await _userHelper.ConfirmEmailAsync(user, myToken);

                    return View();
                }

                this.ViewBag.Message = "Error while resetting the password.";
                return View(model);
            }

            this.ViewBag.Message = "User not found.";
            return View(model);
        }


        public IActionResult NotAuthorized()
        {
            return View();
        }


        [HttpPost]
        [Route("Account/GetCitiesAsync")]
        public async Task<JsonResult> GetCitiesAsync(int countryId)
        {
            var country = await _countryRepository.GetCountryWithCitiesAsync(countryId);
            return Json(country.Cities.OrderBy(c => c.Name));
        }
    }
}
