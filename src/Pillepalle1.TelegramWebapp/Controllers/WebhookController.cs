using System.Threading.Tasks;
using Telegram.Bot.Types;
using Microsoft.AspNetCore.Mvc;

using Pillepalle1.TelegramWebapp.Model.Bot;

namespace Pillepalle1.TelegramWebapp.Controllers
{
    public class WebhookController : Controller
    {
        private readonly ITelegramBotUpdateHandler _telegramBotService;

        public WebhookController(ITelegramBotUpdateHandler telegramBotService)
        {
            _telegramBotService = telegramBotService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update update)
        {    
            await _telegramBotService.HandleUpdateAsync(update);
            return Ok();
        }
    }
}