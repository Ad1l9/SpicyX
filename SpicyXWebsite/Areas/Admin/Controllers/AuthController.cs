using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SpicyXWebsite.Areas.Admin.ViewModel;
using SpicyXWebsite.Models;

namespace SpicyXWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public AuthController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVm userVm)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            AppUser user = new()
            {
                UserName = userVm.UserName,
                Email = userVm.Email,
                Name = userVm.Name,
                Surname = userVm.Surname,
            };

            IdentityResult result = await _userManager.CreateAsync(user, userVm.Password);
            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(String.Empty, error.Description);
                }
                return View(userVm);
            }
            return RedirectToAction(nameof(Login));
        }


        public IActionResult Login()
        {
            return View();
        }
    }
}
