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
            Emoji reactionEmote = new Emoji(reaction);

            var messageReactions = new MessageReaction()
            {
                Message = message.ToLower(),
                Reaction = $"{reactionEmote}",
                GuildId = _dbContext.Guilds.FirstOrDefault(g => g.GuildId == Context.Guild.Id).Id,
            };
            await _dbContext.MessageReactions.AddAsync(messageReactions);
            await _dbContext.SaveChangesAsync();

            var embed = new EmbedBuilder()
                .WithTitle("Dodawanie reakcji")
                .WithColor(new Color(242, 75, 63))
                .AddField($"Wiadomość:", $"{message}")
                .AddField($"Reakcja:", $"{reaction}");

            await ReplyAsync(embed: embed.Build());
        }

        [Command("ReactionList")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task ReactionList()
        {
            var guildTableId = _dbContext.Guilds.FirstOrDefault(g => g.GuildId == Context.Guild.Id).Id;

            var messsageReactions = _dbContext.MessageReactions.Where(m => m.GuildId == guildTableId).ToList<MessageReaction>();

            if (!messsageReactions.Any())
            {
                await ReplyAsync("Brak danych w bazie");
                return;
            }

            var embed = new EmbedBuilder()
                .WithTitle("Lista dodanych wiadomości i reakcji")
                .WithColor(new Color(242, 75, 63));
            foreach (var mr in messsageReactions)
            {
                embed.AddField($"ID: {mr.Id}", $"Wiadomość: {mr.Message}\nReakcja: {mr.Reaction}");
            }

            await ReplyAsync(embed: embed.Build());
        }

        [Command("ReactionDelete")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task ReactionDelete(int id)
        {
            var guildTableId = _dbContext.Guilds.FirstOrDefault(g => g.GuildId == Context.Guild.Id).Id;

            var messageReaction = _dbContext.MessageReactions.FirstOrDefault(r => r.Id == id && r.GuildId == guildTableId);

            if (messageReaction == null)
            {
                var embedError = new EmbedBuilder()
                .WithTitle("Error")
                .WithDescription("Brak takiego ID w bazie")
                .WithColor(new Color(242, 75, 63));

                await ReplyAsync(embed: embedError.Build());
                return;
            }

            _dbContext.MessageReactions.Remove(messageReaction);
            await _dbContext.SaveChangesAsync();

            var embed = new EmbedBuilder()
                .WithTitle("Usunięto reakcje")
                .WithColor(new Color(242, 75, 63))
                .AddField($"ID: {messageReaction.Id}", $"Wiadomość: {messageReaction.Message}\nReakcja: {messageReaction.Reaction}");

            await ReplyAsync(embed: embed.Build());
        }
    }
}
