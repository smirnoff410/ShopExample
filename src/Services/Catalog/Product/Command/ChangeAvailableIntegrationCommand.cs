using System;
using Catalog.Services.DatabaseContext;
using Common.Models.Product;
using Common.Services.Command;

namespace Catalog.Product.Command
{
    public class ChangeAvailableIntegrationCommand : Common.Services.Command.Command
    {
        private readonly IServiceProvider serviceProvider;
        private readonly DecreaseAvailableCountProductMessage message;

        public ChangeAvailableIntegrationCommand(IServiceProvider serviceProvider, DecreaseAvailableCountProductMessage message)
        {
            this.serviceProvider = serviceProvider;
            this.message = message;
        }
        public override CommandResponse Execute()
        {
            var context = serviceProvider.GetRequiredService<CatalogServiceDbContext>();
            var products = context.Products.Where(x => message.Products.Select(x => x.ProductId).Contains(x.Id)).ToList();
            products.ForEach(x =>
            {
                x.AvailableStock -= message.Products.FirstOrDefault(c => c.ProductId == x.Id).ProductCount;
            });
            context.SaveChanges();

            return new CommandResponse { Success = true };
        }
    }
}

