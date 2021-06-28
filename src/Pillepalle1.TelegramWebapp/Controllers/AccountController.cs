using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Pillepalle1.TelegramWebapp.Model.Identity;
using Telegram.Bot.Extensions.LoginWidget;

namespace Pillepalle1.TelegramWebapp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _config;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(IConfiguration config,
                                    SignInManager<ApplicationUser> signInManager,
                                    UserManager<ApplicationUser> userManager)
        {
            _config = config;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            await Task.CompletedTask;

            var botname = _config["BOT_NAME"];
            var fqdn = _config["FQDN"];

            ViewBag.WidgetEmbedCode = WidgetEmbedCodeGenerator.GenerateRedirectEmbedCode(
                _config["BOT_NAME"],
                $"https://{fqdn}{Url.Action("tgcallback", "account")}",
                ButtonStyle.Large,
                true,
                true);

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Tgcallback(
            string id,
            string first_name,
            string username,
            string photo_url,
            string auth_date,
            string hash)
        {
            // attempt to authenticate the login
            var token = _config["BOT_TOKEN"];
            var loginWidget = new LoginWidget(token);
            var auth = loginWidget.CheckAuthorization(new SortedDictionary<string, string>()
            {
                {"id",id},
                {"first_name", first_name},
                {"username", username},
                {"photo_url", photo_url},
                {"auth_date", auth_date},
                {"hash", hash}
            });

            // if the authorization was successful, create the user (if not exist) and sign in
            if (auth == Authorization.Valid)
            {
                var user = await _userManager.FindByNameAsync($"tg{id}");
                
                if (null == user)
                {
                    user = new ApplicationUser()
                    {
                        UserName = $"tg{id}",

                        TelegramNativeId = long.Parse(id),
                        TelegramUserName = username,
                        FirstName = first_name,
                        PhotoUrl = photo_url
                    };

                    var result = await _userManager.CreateAsync(user);
                    if (!result.Succeeded)
                    {
                        ViewBag.ErrorTitle = "Internal error";
                        ViewBag.ErrorMessage = $"Failed to create user tg{id}";
                        return View("Error");
                    }

                    user = await _userManager.FindByNameAsync($"tg{id}");
                    if (null == user)
                    {
                        ViewBag.ErrorTitle = "Internal error";
                        ViewBag.ErrorMessage = $"Failed to create user tg{id}";
                        return View("Error");
                    }
                }

                await _signInManager.SignInAsync(user, true);
            }

            // return him back to home/index where he will be redirected to login,
            // if the login was unsuccessful
            return RedirectToAction("index", "home");
        }
    }
}