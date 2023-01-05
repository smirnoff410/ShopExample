using System;
using Common.Models.Product;
using Common.Models.User;
using Common.Services.Command;
using Common.Services.MessageQueue;

namespace Catalog.Product.Command
{
    public class CreateProductIntegrationCommand : Common.Services.Command.CommandIntegrate
    {

        private readonly IMessageQueue _messageQueue;

        private Entity.Product _dto;

        public CreateProductIntegrationCommand(IServiceProvider provider)
        {
            _messageQueue = provider.GetRequiredService<IMessageQueue>();
        }

        public override CommandResponse Execute()
        {
            _messageQueue.Publish<ProductIntegrationMessage>("product", new ProductIntegrationMessage
            {
                Id = _dto.Id,
                Name = _dto.Name,
                Price = _dto.Price,
                ImageUrl = _dto.ImageUrl
            });
            return new CommandResponse();
        }

        public override void SetData(object data)
        {
            _dto = (Entity.Product)data;
        }
    }
}

