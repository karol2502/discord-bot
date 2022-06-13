using Discord;
using Discord.Commands;
using Discord.WebSocket;
using LegendaryBot.Database;
using LegendaryBot.Entities;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace LegendaryBot.Services
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IConfigurationRoot _config;
        private readonly IServiceProvider _provider;
        private readonly LegendaryDbContext _dbContext;

        // DiscordSocketClient, CommandService, IConfigurationRoot, and IServiceProvider are injected automatically from the IServiceProvider
        public CommandHandler(
            DiscordSocketClient client,
            CommandService commands,
            IConfigurationRoot config,
            IServiceProvider provider,
            LegendaryDbContext dbContext)
        {
            _client = client;
            _commands = commands;
            _config = config;
            _provider = provider;
            _dbContext = dbContext;

            _client.MessageReceived += OnMessageReceivedAsync;
        }

        private async Task OnMessageReceivedAsync(SocketMessage s)
        {
            var msg = s as SocketUserMessage;     // Ensure the message is from a user/bot
            if (msg == null) return;
            if (msg.Author.Id == _client.CurrentUser.Id) return;     // Ignore self when checking commands

            var context = new SocketCommandContext(_client, msg);     // Create the command context

            int argPos = 0;     // Check if the message has a valid command prefix
            if (msg.HasStringPrefix(_config["prefix"], ref argPos) || msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                var result = await _commands.ExecuteAsync(context, argPos, _provider);     // Execute the command

                if (!result.IsSuccess)     // If not successful, reply with the error.
                    await context.Channel.SendMessageAsync(result.ToString());
            }
            else
            {
                var id = _dbContext.Guilds.FirstOrDefault(g => g.GuildId == context.Guild.Id).Id;

                var responses = _dbContext.MessageResponses.Where(m => m.GuildId == id).ToList<MessageResponse>();
                foreach (var response in responses)
                {
                    if (msg.Content.ToLower().Contains(response.Message))
                    {
                        await context.Channel.TriggerTypingAsync();
                        await context.Channel.SendMessageAsync(response.Response);
                    }
                }

                var reactions = _dbContext.MessageReactions.Where(m => m.GuildId == id).ToList<MessageReaction>();
                foreach (var reaction in reactions)
                {
                    if (msg.Content.ToLower().Contains(reaction.Message))
                    {
                        await s.AddReactionAsync(Emote.Parse(reaction.Reaction));
                    }
                }
            }
        }
    }
}
