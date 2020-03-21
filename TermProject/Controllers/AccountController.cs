using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TermProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;
using Microsoft.Extensions.Logging;
using TermProject.Repositories;

namespace TermProject.Controllers
{

    public class AccountController : Controller
    {
        private UserManager<Player> userManager;
        private SignInManager<Player> signInManager;
        private IWebHostEnvironment webHostEnvironment;
        private ILogger<RegisterModel> _logger;
        private IRepository repo;

        public AccountController(UserManager<Player> userMgr,
            IWebHostEnvironment hostEnvironment,
            ILogger<RegisterModel> logger,
                SignInManager<Player> signinMgr,
                IRepository r)
        {
            userManager = userMgr;
            signInManager = signinMgr;
            _logger = logger;
            webHostEnvironment = hostEnvironment;
            repo = r;
        }


        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            if (returnUrl == null)
            {
                returnUrl = "/";
            }
            ViewBag.back = true;
            ViewBag.returnUrl = returnUrl;
            LoginModel model = new LoginModel()
            {
                ReturnUrl = returnUrl
            };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel details, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                Player user = await userManager.FindByEmailAsync(details.Email);
                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result =
                            await signInManager.PasswordSignInAsync(
                                user, details.Password, false, false);
                    if (result.Succeeded)
                    {
                        if (!string.IsNullOrEmpty(returnUrl))//goes back to original url
                            return Redirect(returnUrl ?? "/");
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError(nameof(LoginModel.Email),
                    "Invalid user or password");
            }
            return View(details);
        }
        [Authorize]
        public IActionResult LogoutView()
        {
            ViewBag.back = true;
            return View("Logout");
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Registration(string returnUrl)
        {
            ViewBag.back = true;
            if (returnUrl == null)
            {
                returnUrl = "/";
            }
            ViewBag.returnUrl = returnUrl;
            RegisterModel model = new RegisterModel()
            {
                ReturnUrl = returnUrl
            };
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Registration(RegisterModel model, string returnUrl)
        {
            if (model.ProfileImage.Length < 5000000)
            {
                ModelState.AddModelError(string.Empty, "im not amazon. too big of a pic");
            }
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(model.ProfileImage);
                var user = new Player
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Score = 1,
                    IsDueling = false,
                    Voted = false,
                    ProfilePicture = uniqueFileName,
                    DuelCard = repo.WhiteCards[0]
                };
                var result = await userManager.CreateAsync(user, model.Password);
                result = await userManager.AddToRoleAsync(user, "Member");


                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            ModelState.AddModelError("Validation", "Card Added ");
            return View(model);
        }
        private string UploadedFile(IFormFile image)
        {
            string uniqueFileName = null;

            if (image != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}
