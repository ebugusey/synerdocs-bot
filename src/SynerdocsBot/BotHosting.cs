using System;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SynerdocsBot
{
    class BotHosting : IHostedService
    {
        readonly DiscordClient _botClient;
        readonly IServiceProvider _serviceProvider;
        readonly ILogger<BotHosting> _logger;

        public BotHosting(
            DiscordClient botClient,
            IServiceProvider serviceProvider,
            ILogger<BotHosting> logger)
        {
            _botClient = botClient;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var commands = _botClient.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefixes = new[] { "!" },
                Services = _serviceProvider,
            });

            commands.CommandErrored += LogCommandError;

            commands.RegisterCommands<CommandsModule>();

            await _botClient.ConnectAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _botClient.DisconnectAsync();
        }

        Task LogCommandError(CommandsNextExtension sender, CommandErrorEventArgs e)
        {
            _logger.LogError(e.Exception,
                "{Command} thrown an exception",
                e.Command);

            return Task.CompletedTask;
        }
    }
}
