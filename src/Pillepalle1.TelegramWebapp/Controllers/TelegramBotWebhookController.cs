using System.Threading.Tasks;
using Telegram.Bot.Types;
using Microsoft.AspNetCore.Mvc;

using Pillepalle1.TelegramWebapp.Model.TelegramBot;

namespace Pillepalle1.TelegramWebapp.Controllers
{
    public class TelegramBotWebhookController : Controller
    {
        private readonly ITelegramBotUpdateHandler _telegramBotService;

        public TelegramBotWebhookController(ITelegramBotUpdateHandler telegramBotService)
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