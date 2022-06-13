using LegendaryBot.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace LegendaryBot.Database
{
    public class LegendaryDbContext : DbContext
    {
        public DbSet<Guild>? Guilds { get; set; }
        public DbSet<MessageResponse>? MessageResponses { get; set; }
        public DbSet<MessageReaction>? MessageReactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Guild>()
                .Property(g => g.GuildId)
                .IsRequired();

            modelBuilder.Entity<MessageResponse>()
                .Property(m => m.Message)
                .IsRequired();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string[] paths = { "LegendaryBot.db" };
            string fullPath = Path.Combine(paths);
            optionsBuilder.UseSqlite($"Data Source={fullPath}", options =>
            {
                options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });
            base.OnConfiguring(optionsBuilder);
        }
    }
}
