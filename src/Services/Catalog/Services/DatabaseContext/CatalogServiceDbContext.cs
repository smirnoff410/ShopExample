using System;
using Common.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Catalog.Services.DatabaseContext
{
    using Catalog.Product.Entity;
    public class CatalogServiceDbContext : DbContext
    {
        private readonly IOptions<DatabaseSettings> _options;
        private readonly ILogger<CatalogServiceDbContext> _logger;

        public DbSet<Product> Products => Set<Product>();

        public CatalogServiceDbContext(IOptions<DatabaseSettings> options, ILogger<CatalogServiceDbContext> logger)
        {
            _options = options;
            _logger = logger;

            if (!Database.CanConnect())
            {
                Database.EnsureCreated();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_options.Value.ConnectionString);
            _logger.LogInformation($"Connect to database: {_options.Value.ConnectionString}");
        }
    }
}

