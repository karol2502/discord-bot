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

            await ReplyAsync($"Message: {message}\nResponse: {response}\nAdded!");
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

            string message = "";

            foreach (var mr in messageResponses)
            {
                message += $"Id: {mr.Id}\tMessage: {mr.Message}\tResponse: {mr.Response}\n";
            }

            await ReplyAsync(message);
        }

        [Command("ResponseDelete")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task ResponseDelete(int id)
        {
            var guildTableId = _dbContext.Guilds.FirstOrDefault(g => g.GuildId == Context.Guild.Id).Id;

            var messageResponse = _dbContext.MessageResponses.FirstOrDefault(r => r.Id == id && r.GuildId == guildTableId);

            if (messageResponse == null)
            {
                await ReplyAsync("Brak takiego rekordu");
                return;
            }

            _dbContext.MessageResponses.Remove(messageResponse);
            await _dbContext.SaveChangesAsync();

            await ReplyAsync($"Usunięto odpowiedź");
        }
    }
}
