using System;
using Catalog.Product.DTO;
using Catalog.Services.DatabaseContext;
using Common.Services.Command;

namespace Catalog.Product.Command
{
    public class CreateProductCommand : Common.Services.Command.Command
    {
        private readonly CreateProductDTO _dto;
        private readonly CatalogServiceDbContext _context;

        public CreateProductCommand(IServiceProvider provider, CreateProductDTO dto)
        {
            _dto = dto;
            _context = provider.GetRequiredService<CatalogServiceDbContext>();
        }

        public override CommandResponse Execute()
        {
            var product = new Entity.Product
            {
                Name = _dto.Name,
                Description = _dto.Description,
                Price = _dto.Price,
                AvailableStock = _dto.AvailableStock,
                ImageUrl = _dto.ImageUrl
            };
            _context.Products.Add(product);

            _context.SaveChanges();

            return new CommandResponse { Success = true, Response = product };
        }
    }
}

