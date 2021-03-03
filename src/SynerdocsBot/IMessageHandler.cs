using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;

namespace SynerdocsBot
{
    interface IMessageHandler
    {
        bool CanHandle(MessageCreateEventArgs args);
        Task Handle(MessageCreateEventArgs args);
    }
}
