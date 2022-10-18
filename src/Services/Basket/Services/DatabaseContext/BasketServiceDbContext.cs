using System;
using Common.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Basket.Services.DatabaseContext
{
    using Basket.Entity;
    using User.Entity;
    public class BasketServiceDbContext : DbContext
    {
        private readonly IOptions<DatabaseSettings> _options;
        private readonly ILogger<BasketServiceDbContext> _logger;

        public DbSet<Basket> Baskets => Set<Basket>();
        public DbSet<User> Users => Set<User>();

        public BasketServiceDbContext(IOptions<DatabaseSettings> options, ILogger<BasketServiceDbContext> logger)
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Basket>()
                .HasOne(x => x.User)
                .WithOne()
                .HasForeignKey<Basket>(x => x.UserId);

            modelBuilder
                .Entity<Basket>()
                .HasMany(x => x.Products)
                .WithMany(x => x.Baskets)
                .UsingEntity(x => x.ToTable("BasketProduct"));
        }
    }
}

