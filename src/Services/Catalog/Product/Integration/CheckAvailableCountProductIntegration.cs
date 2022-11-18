using System;
using Catalog.Services.DatabaseContext;
using Common.Models.Product;
using Common.Services.Command;

namespace Catalog.Product.Integration
{
    public class CheckAvailableCountProductIntegration : Common.Services.Command.Command
    {
        private readonly CatalogServiceDbContext _context;
        private readonly CheckProductIntegrationMessage _message;

        public CheckAvailableCountProductIntegration(IServiceScope scope, CheckProductIntegrationMessage message)
        {
            _context = scope.ServiceProvider.GetRequiredService<CatalogServiceDbContext>();
            _message = message;
        }

        public override CommandResponse Execute()
        {
            var product = _context.Products.Find(_message.ProductId);
            if (product.AvailableStock - _message.ProductCount >= 0)
                return new CommandResponse { Response = true };
            else
                return new CommandResponse { Response = false };
        }
    }
}

