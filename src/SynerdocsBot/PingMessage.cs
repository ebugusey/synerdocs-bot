using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace SynerdocsBot
{
    public class PingMessage : IMessageHandler
    {
        public bool CanHandle(MessageCreateEventArgs args)
        {
            return args.Message.Content == "123";
        }

        public async Task Handle(MessageCreateEventArgs args)
        {
            await new DiscordMessageBuilder()
                .WithContent("321")
                .WithReply(args.Message.Id)
                .SendAsync(args.Channel);
        }
    }
}
