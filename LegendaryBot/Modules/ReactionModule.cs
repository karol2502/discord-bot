using Discord;
using Discord.Commands;
using LegendaryBot.Database;
using LegendaryBot.Entities;

namespace LegendaryBot.Modules
{
    public class ReactionModule : ModuleBase<SocketCommandContext>
    {
        private readonly LegendaryDbContext _dbContext;

        public ReactionModule(LegendaryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Command("ReactionAdd")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task ReactionAdd([Remainder] string messageAndReaction)
        {
            string[] text = messageAndReaction.Split(';');
            string message = text[0].Trim();
            string reaction = text[1].Trim();

            var messageReactions = new MessageReaction()
            {
                Message = message.ToLower(),
                Reaction = reaction,
                GuildId = _dbContext.Guilds.FirstOrDefault(g => g.GuildId == Context.Guild.Id).Id,
            };
            await _dbContext.MessageReactions.AddAsync(messageReactions);
            await _dbContext.SaveChangesAsync();

            await ReplyAsync($"Message: {message}\nReaction: {reaction}\nAdded!");
        }

        [Command("ReactionList")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task ReactionList()
        {
            var id = _dbContext.Guilds.FirstOrDefault(g => g.GuildId == Context.Guild.Id).Id;

            var messsageReactions = _dbContext.MessageReactions.Where(m => m.GuildId == id).ToList<MessageReaction>();

            if (!messsageReactions.Any())
            {
                await ReplyAsync("Brak danych w bazie");
                return;
            }

            string message = "";

            foreach (var mr in messsageReactions)
            {
                message += $"Id: {mr.Id}\tMessage: {mr.Message}\tResponse: {mr.Reaction}\n";
            }

            await ReplyAsync(message);
        }

        [Command("ReactionDelete")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task ReactionDelete(int id)
        {
            var guildTableId = _dbContext.Guilds.FirstOrDefault(g => g.GuildId == Context.Guild.Id).Id;

            var messageReaction = _dbContext.MessageReactions.FirstOrDefault(r => r.Id == id && r.GuildId == guildTableId);

            if (messageReaction == null)
            {
                await ReplyAsync("Brak takiego rekordu");
                return;
            }

            _dbContext.MessageReactions.Remove(messageReaction);
            await _dbContext.SaveChangesAsync();

            await ReplyAsync($"Usunięto reakcje");
        }
    }
}
