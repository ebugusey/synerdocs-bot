using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using Microsoft.Extensions.Hosting;

namespace SynerdocsBot
{
    class BotHosting : IHostedService
    {
        readonly DiscordClient _botClient;

        public BotHosting(DiscordClient botClient)
        {
            _botClient = botClient;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _botClient.ConnectAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _botClient.DisconnectAsync();
        }
    }
}
