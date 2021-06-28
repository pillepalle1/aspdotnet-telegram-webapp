using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace Pillepalle1.TelegramWebapp.Services
{
    public class TelegramBotHostedService : IHostedService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IConfiguration _config;

        public TelegramBotHostedService(IConfiguration config, ITelegramBotClient botClient)
        {
            _config = config;
            _botClient = botClient;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var fqdn = _config["FQDN"];
            var token = _config["BOT_TOKEN"];


            await _botClient.SetWebhookAsync($"https://{fqdn}/bot/{token}");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _botClient.DeleteWebhookAsync();
        }
    }
}