using Discord;
using Discord.Commands;
using Discord.WebSocket;
using LegendaryBot.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace LegendaryBot.Services
{
    public class StartupService
    {
        private readonly IServiceProvider _provider;
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly LegendaryDbContext _dbContext;
        private readonly IConfigurationRoot _config;
        private readonly LegendaryDbSeeder _legendaryDbSeeder;

        // DiscordSocketClient, CommandService, and IConfigurationRoot are injected automatically from the IServiceProvider
        public StartupService(
            IServiceProvider provider,
            DiscordSocketClient discord,
            CommandService commands,
            IConfigurationRoot config,
            LegendaryDbSeeder legendaryDbSeeder,
            LegendaryDbContext dbContext)
        {
            _provider = provider;
            _config = config;
            _legendaryDbSeeder = legendaryDbSeeder;
            _client = discord;
            _commands = commands;
            _dbContext = dbContext;

            _client.Ready += OnReady;      // Added function invoked on client ready
        }

        public async Task StartAsync()
        {
            string discordToken = _config["token"];     // Get the discord token from the config file
            if (string.IsNullOrWhiteSpace(discordToken))
                throw new Exception("Please enter your bot's token into the `config.json` file found in the applications root directory.");

            await _client.LoginAsync(TokenType.Bot, discordToken);     // Login to discord
            await _client.StartAsync();

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);     // Load commands and modules into the command service            
        }

        private async Task OnReady()
        {
            try
            {
                Console.WriteLine("Try migrate");
                await _dbContext.Database.MigrateAsync();
            }
            catch
            {
                Console.WriteLine("Migrate not succeded");
            }
            try
            {
                Console.WriteLine("Try create");
                await _dbContext.Database.EnsureCreatedAsync();
            }
            catch
            {
                Console.WriteLine("Create not succeded");
            }
            await _legendaryDbSeeder.Seed();
            Console.WriteLine("Database connected!");
        }
    }
}
