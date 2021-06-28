using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Pillepalle1.TelegramWebapp.Model.TelegramBot
{
    public interface ITelegramBotUpdateHandler
    {
        Task HandleUpdateAsync(Update update);
        
    }
}