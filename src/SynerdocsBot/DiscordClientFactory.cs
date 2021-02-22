using System;
using DSharpPlus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SynerdocsBot
{
    class DiscordClientFactory
    {
        const string TokenConfig = "BotToken";

        readonly IConfiguration _configuration;
        readonly ILoggerFactory _loggerFactory;

        public DiscordClientFactory(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            _configuration = configuration;
            _loggerFactory = loggerFactory;
        }

        public DiscordClient Create()
        {
            var token = _configuration[TokenConfig];
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new InvalidOperationException($"No {TokenConfig} found in configuration.");
            }

            var config = new DiscordConfiguration
            {
                Token = token,
                TokenType = TokenType.Bot,
                LoggerFactory = _loggerFactory,
            };
            var client = new DiscordClient(config);

            return client;
        }
    }
}
