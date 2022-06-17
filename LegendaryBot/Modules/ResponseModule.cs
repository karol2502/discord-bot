using Discord;
using Discord.Commands;
using LegendaryBot.Database;
using LegendaryBot.Entities;

namespace LegendaryBot.Modules
{
    public class ResponseModule : ModuleBase<SocketCommandContext>
    {
        private readonly LegendaryDbContext _dbContext;

        public ResponseModule(LegendaryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Command("ResponseAdd")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task ResposneAdd([Remainder] string messageAndResponse)
        {
            string[] text = messageAndResponse.Split(';');
            string message = text[0].Trim();
            string response = text[1].Trim();

            var messageResponse = new MessageResponse()
            {
                Message = message.ToLower(),
                Response = response,
                GuildId = _dbContext.Guilds.FirstOrDefault(g => g.GuildId == Context.Guild.Id).Id,
            };
            await _dbContext.MessageResponses.AddAsync(messageResponse);
            await _dbContext.SaveChangesAsync();

            var embed = new EmbedBuilder()
                .WithTitle("Dodawanie odpowiedzi")
                .WithColor(new Color(242, 75, 63))
                .AddField($"Wiadomość:", $"{message}")
                .AddField($"Odpowiedź:", $"{response}");

            await ReplyAsync(embed: embed.Build());
        }

        [Command("ResponseList")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task ResponseList()
        {
            var guildTableId = _dbContext.Guilds.FirstOrDefault(g => g.GuildId == Context.Guild.Id).Id;

            var messageResponses = _dbContext.MessageResponses.Where(m => m.GuildId == guildTableId).ToList<MessageResponse>();

            if (!messageResponses.Any())
            {
                await ReplyAsync("Brak danych w bazie");
                return;
            }

            var embed = new EmbedBuilder()
                .WithTitle("Lista dodanych wiadomości i odpowiedzi")
                .WithColor(new Color(242, 75, 63));
            foreach (var mr in messageResponses)
            {
                embed.AddField($"ID: {mr.Id}", $"Wiadomość: {mr.Message}\nOdpowiedź: {mr.Response}");
            }

            await ReplyAsync(embed: embed.Build());
        }

        [Command("ResponseDelete")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task ResponseDelete(int id)
        {
            var guildTableId = _dbContext.Guilds.FirstOrDefault(g => g.GuildId == Context.Guild.Id).Id;

            var messageResponse = _dbContext.MessageResponses.FirstOrDefault(r => r.Id == id && r.GuildId == guildTableId);

            if (messageResponse == null)
            {
                var embedError = new EmbedBuilder()
                .WithTitle("Error")
                .WithDescription("Brak takiego ID w bazie")
                .WithColor(new Color(242, 75, 63));

                await ReplyAsync(embed: embedError.Build());
                return;
            }

            _dbContext.MessageResponses.Remove(messageResponse);
            await _dbContext.SaveChangesAsync();

            var embed = new EmbedBuilder()
                .WithTitle("Usunięto odpowiedź")
                .WithColor(new Color(242, 75, 63))
                .AddField($"ID: {messageResponse.Id}", $"Wiadomość: {messageResponse.Message}\nReakcja: {messageResponse.Response}");

            await ReplyAsync(embed: embed.Build());
        }
    }
}
