using System;
using Basket.Services.DatabaseContext;
using Common.Models.Product;
using Common.Services.Command;

namespace Basket.Product.Integration
{
    public class CreateProductIntegration : Command
    {
        private readonly BasketServiceDbContext _context;
        private readonly ProductIntegrationMessage _message;

        public CreateProductIntegration(IServiceScope scope, ProductIntegrationMessage message)
        {
            _context = scope.ServiceProvider.GetRequiredService<BasketServiceDbContext>();
            _message = message;
        }

        public override CommandResponse Execute()
        {
            var product = new Entity.Product
            {
                Id = _message.Id,
                Name = _message.Name,
                Price = _message.Price,
                ImageUrl = _message.ImageUrl
            };

            _context.Products.Add(product);
            _context.SaveChanges();

            return new CommandResponse { Success = true };
        }
    }
}

