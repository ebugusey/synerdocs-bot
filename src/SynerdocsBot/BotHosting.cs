using System;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using Microsoft.Extensions.Hosting;

namespace SynerdocsBot
{
    class BotHosting : IHostedService
    {
        readonly DiscordClient _botClient;
        readonly IServiceProvider _serviceProvider;

        public BotHosting(
            DiscordClient botClient,
            IServiceProvider serviceProvider)
        {
            _botClient = botClient;
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var commands = _botClient.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefixes = new[] { "!" },
                Services = _serviceProvider,
            });

            commands.RegisterCommands<CommandsModule>();

            await _botClient.ConnectAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _botClient.DisconnectAsync();
        }
    }
}
