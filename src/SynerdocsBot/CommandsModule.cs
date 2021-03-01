using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Unsplash;
using Unsplash.Requests;

namespace SynerdocsBot
{
    public class CommandsModule : BaseCommandModule
    {
        readonly Client _client;

        public CommandsModule(UnsplashClientFactory clientFactory)
        {
            _client = clientFactory.Create();
        }

        [Command("cat"), Description("Получить фото котиков.")]
        [Aliases("кошка", "котики")]
        public async Task GetRandomPhoto(CommandContext ctx)
        {
            var request = new GetRandomPhotoRequest(query: "cat");

            var photo = await _client.GetRandomPhotoAsync(request);
            if (photo == null)
            {
                await ctx.Channel.SendMessageAsync("По запросу ничего не найдено");
            }

            var embed = new DiscordEmbedBuilder
            {
                Title = "кошка",
                ImageUrl = photo?.urls.full.AbsoluteUri
            };

            await ctx.RespondAsync(embed);
        }
    }
}
