using System;
using Catalog.Product.DTO;
using Catalog.Services.DatabaseContext;
using Common.Services.Command;

namespace Catalog.Product.Command
{
    public class ChangeAvailableCommand : Common.Services.Command.Command
    {
        private readonly ChangeAvailableDTO _dto;
        private readonly CatalogServiceDbContext _context;
        private readonly int _id;

        public ChangeAvailableCommand(IServiceProvider provider, ChangeAvailableDTO dto, int id)
        {
            _context = provider.GetRequiredService<CatalogServiceDbContext>();
            _dto = dto;
            _id = id;
        }

        public override CommandResponse Execute()
        {
            var product = _context.Products.Find(_id);

            product.AvailableStock = _dto.Availabe;

            _context.SaveChanges();

            return new CommandResponse { Success = true };
        }
    }
}

