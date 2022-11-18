using System;
using Catalog.Services.DatabaseContext;
using Common.Models.Product;
using Common.Services.Command;

namespace Catalog.Product.Integration
{
    public class DecreaseAvailableCountProductIntegration : Common.Services.Command.Command
    {
        private readonly IServiceScope _scope;
        private readonly DecreaseAvailableCountProductMessage _message;
        private readonly CatalogServiceDbContext _context;

        public DecreaseAvailableCountProductIntegration(IServiceScope scope, DecreaseAvailableCountProductMessage message)
        {
            _scope = scope;
            _message = message;
            _context = scope.ServiceProvider.GetRequiredService<CatalogServiceDbContext>();
        }

        public override CommandResponse Execute()
        {
            foreach(var m in _message.Products)
            {
                var product = _context.Products.Find(m.ProductId);
                product.AvailableStock -= m.ProductCount;
            }
            _context.SaveChanges();

            return new CommandResponse { Success = true };
        }
    }
}

