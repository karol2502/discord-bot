using Discord;
using Discord.Commands;
using LegendaryBot.Database;
using LegendaryBot.Entities;

namespace LegendaryBot.Modules
{
    public class TestModule : ModuleBase<SocketCommandContext>
    {
        private readonly LegendaryDbContext _dbContext;

        public TestModule(LegendaryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Command("ping"), Alias("p")]
        [Summary("Return ping of bot")]
        public async Task Ping()
            => await ReplyAsync("pong");
    }
}
