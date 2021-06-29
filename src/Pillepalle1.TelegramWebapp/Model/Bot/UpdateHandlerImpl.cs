using System.Threading.Tasks;
using Telegram.Bot.Types;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace Pillepalle1.TelegramWebapp.Model.Bot
{
    public class TelegramBotUpdateHandlerImpl : ITelegramBotUpdateHandler
    {
        private readonly ILogger<TelegramBotUpdateHandlerImpl> _logger;
        private readonly ITelegramBotClient _botclient;

        public TelegramBotUpdateHandlerImpl(ILogger<TelegramBotUpdateHandlerImpl> logger,
                                            ITelegramBotClient botclient)
        {
            _logger = logger;
            _botclient = botclient;
        }

        public async Task HandleUpdateAsync(Update update)
        {
            // Simple echo client for textmessages

            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                var m = update.Message;

                if (m.Type == Telegram.Bot.Types.Enums.MessageType.Text)
                {
                    await _botclient.SendTextMessageAsync(m.Chat.Id, m.Text);
                }
            }
        }
    }
}