using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SynerdocsBot
{
    sealed class BotHosting : IHostedService, IDisposable
    {
        readonly DiscordClient _botClient;
        readonly IServiceProvider _serviceProvider;
        readonly ILogger<BotHosting> _logger;
        readonly IEnumerable<IMessageHandler> _messageHandlers;

        public BotHosting(
            DiscordClient botClient,
            IServiceProvider serviceProvider,
            ILogger<BotHosting> logger,
            DiscordClientFactory clientFactory, IEnumerable<IMessageHandler> messageHandlers)
        {
            _botClient = clientFactory.Create();
            _messageHandlers = messageHandlers;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            _botClient.MessageCreated += OnMessageCreated;

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
            ThrowIfDisposed();

            _botClient.MessageCreated -= OnMessageCreated;

            await _botClient.DisconnectAsync();
        }

        Task LogCommandError(CommandsNextExtension sender, CommandErrorEventArgs e)
        {
            _logger.LogError(e.Exception,
                "{Command} thrown an exception",
                e.Command);

            return Task.CompletedTask;
        }

        Task OnMessageCreated(DiscordClient sender, MessageCreateEventArgs e)
        {
            var handlers = _messageHandlers.Where(x => x.CanHandle(e));

            foreach (var handler in handlers)
            {
                _ = handler.Handle(e);
            }

            return Task.CompletedTask;
        }

        #region Disposable

        bool _disposed;

        void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
        }

        void IDisposable.Dispose()
        {
            _disposed = true;

            _botClient.Dispose();
        }

        #endregion
    }
}
