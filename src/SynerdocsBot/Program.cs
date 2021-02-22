using DSharpPlus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using Serilog.Sinks.SystemConsole.Themes;

namespace SynerdocsBot
{
    static class Program
    {
        static void Main(string[] args)
        {
            CreateHost(args).Run();
        }

        static IHost CreateHost(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(ConfigureAppConfig)
                .ConfigureLogging(ConfigureLogging)
                .ConfigureServices(ConfigureServices);

            return builder.Build();
        }

        static void ConfigureServices(IServiceCollection services)
        {
            services.AddHostedService<BotHosting>();
            services.AddTransient<DiscordClientFactory>();
            services.AddSingleton<DiscordClient>(provider =>
            {
                var factory = provider.GetRequiredService<DiscordClientFactory>();
                return factory.Create();
            });
        }

        static void ConfigureLogging(ILoggingBuilder cfg)
        {
            cfg.ClearProviders();

            var serilog = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Verbose()
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext}: {Message:lj}{NewLine}{Exception}",
                    theme: AnsiConsoleTheme.Code)
                .CreateLogger();

            cfg.AddProvider(new SerilogLoggerProvider(serilog));
        }

        static void ConfigureAppConfig(HostBuilderContext host, IConfigurationBuilder config)
        {
            config.Sources.Clear();

            var env = host.HostingEnvironment;

            config.AddYamlFile("appsettings.yaml", optional: false, reloadOnChange: true);
            config.AddYamlFile($"appsettings.{env.EnvironmentName}.yaml", optional: true, reloadOnChange: true);

            if (env.IsDevelopment())
            {
                config.AddUserSecrets(typeof(Program).Assembly, optional: true);
            }

            config.AddEnvironmentVariables();
        }
    }
}
