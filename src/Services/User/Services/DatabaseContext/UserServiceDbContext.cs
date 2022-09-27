using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using User.Config;
using User.User.Entity;

namespace User.Services.DatabaseContext
{
    using User.Entity;

    public class UserServiceDbContext : DbContext
    {
        private readonly IOptions<DatabaseSettings> _options;
        private readonly ILogger<UserServiceDbContext> _logger;

        DbSet<User> Users => Set<User>();

        public UserServiceDbContext(IOptions<DatabaseSettings> options, ILogger<UserServiceDbContext> logger)
        {
            _options = options;
            _logger = logger;

            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_options.Value.ConnectionString);
            _logger.LogInformation($"Connect to database: {_options.Value.ConnectionString}");
        }
    }
}

