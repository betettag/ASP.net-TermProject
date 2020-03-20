using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TermProject.Models;
using Microsoft.AspNetCore.Identity;

namespace TermProject.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private UserManager<Player> userManager;
        private SignInManager<Player> signInManager;

        public AccountController(UserManager<Player> userMgr,
                SignInManager<Player> signinMgr)
        {
            userManager = userMgr;
            signInManager = signinMgr;
        }


        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.back = true;
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel details,
                string returnUrl)
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
                        return Redirect(returnUrl ?? "/");
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
            return RedirectToAction("Index", "Home",
                new { message = "You have been successfully logged out" });
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
