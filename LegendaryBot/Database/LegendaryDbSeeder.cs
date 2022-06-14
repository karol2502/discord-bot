using Discord.WebSocket;
using LegendaryBot.Entities;

namespace LegendaryBot.Database
{
    public class LegendaryDbSeeder
    {
        private readonly LegendaryDbContext _dbContext;
        private readonly DiscordSocketClient _client;

        public LegendaryDbSeeder(LegendaryDbContext dbContext, DiscordSocketClient client)
        {
            _dbContext = dbContext;
            _client = client;
        }

        public async Task Seed()
        {
            if (await _dbContext.Database.CanConnectAsync())
            {
                Console.WriteLine("Seeder, connected to db");
                if (!_dbContext.Guilds.Any())
                {
                    var guilds = GetGuilds();
                    await _dbContext.Guilds.AddRangeAsync(guilds);
                    await _dbContext.SaveChangesAsync();
                }
            }
            else
            {
                throw new Exception();
            }
        }

        private IEnumerable<Guild> GetGuilds()
        {
            var guilds = new List<Guild>();
            foreach (var guild in _client.Guilds)
            {
                guilds.Add(new Guild()
                {
                    GuildId = guild.Id,
                    JoinedTime = DateTime.Now
                });
            }
            return guilds;
        }
    }
}
