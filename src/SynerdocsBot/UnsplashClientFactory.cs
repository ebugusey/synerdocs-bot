using System;
using Microsoft.Extensions.Configuration;
using Unsplash;

namespace SynerdocsBot
{
    public class UnsplashClientFactory
    {
        const string UnspashToken = "UnsplashToken";

        readonly IConfiguration _configuration;

        public UnsplashClientFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Client Create()
        {
            var token = _configuration[UnspashToken];
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new InvalidOperationException($"No {UnspashToken} found in configuration.");
            }

            var client = new Client(token);

            return client;
        }
    }
}
