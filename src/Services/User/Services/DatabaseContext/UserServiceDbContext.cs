using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using User.User.Entity;

namespace User.Services.DatabaseContext
{
    using Common.Settings;
    using User.Entity;

    public class UserServiceDbContext : DbContext
    {
        private readonly IOptions<DatabaseSettings> _options;
        private readonly ILogger<UserServiceDbContext> _logger;

        public DbSet<User> Users => Set<User>();

        public UserServiceDbContext(IOptions<DatabaseSettings> settings, ILogger<UserServiceDbContext> logger, DbContextOptions<UserServiceDbContext> options) : base(options)
        {
            _options = settings;
            _logger = logger;

            if (!Database.CanConnect())
            {
                Database.EnsureCreated();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(_options.Value.ConnectionString).LogTo(Console.WriteLine, LogLevel.Information);
                _logger.LogInformation($"Connect to database: {_options.Value.ConnectionString}");
            }
        }
    }
}

