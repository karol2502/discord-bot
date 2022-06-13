using Discord;
using Discord.Commands;
using LegendaryBot.Database;
using LegendaryBot.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendaryBot.Modules
{
    public class OnMessageManage : ModuleBase<SocketCommandContext>
    {
        private readonly LegendaryDbContext _dbContext;

        public OnMessageManage(LegendaryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Command("AddResponse")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task AddResposne([Remainder] string messageAndResponse)
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

        [Command("AddReaction")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task AddReaction([Remainder] string messageAndReaction)
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
    }
}
