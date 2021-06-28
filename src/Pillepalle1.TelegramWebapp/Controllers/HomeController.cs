using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pillepalle1.TelegramWebapp.Model.Identity;
using Pillepalle1.TelegramWebapp.Viewmodels;

namespace Pillepalle1.TelegramWebapp.Controllers
{
    public class HomeController : Controller
    {
        private SignInManager<ApplicationUser> _signInManager;

        public HomeController(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
          if (_signInManager.IsSignedIn(User))
            {
                var user = await _signInManager.UserManager.GetUserAsync(User);

                var model = new HomeIndexViewmodel()
                {
                    TgNativeId = user.TelegramNativeId.ToString(),
                    FirstName = user.FirstName,
                    UserName = user.TelegramUserName,
                    PhotoUrl = user.PhotoUrl
                };

                return View(model);
            }
            else
            {
                return RedirectToAction("login", "account");
            }
        }
    }
}