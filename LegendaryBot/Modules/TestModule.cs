using Discord.Commands;

namespace LegendaryBot.Modules
{
    public class TestModule : ModuleBase<SocketCommandContext>
    {
        [Command("ping"), Alias("p")]
        [Summary("Return ping of bot")]
        public Task Ping()
            => ReplyAsync("pong");


    }
}
