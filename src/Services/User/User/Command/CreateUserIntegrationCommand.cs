using System;
using Common.Models.User;
using Common.Services.Command;
using Common.Services.MessageQueue;

namespace User.User.Command
{
    using global::User.User.DTO;
    using User.Entity;
    public class CreateUserIntegrationCommand : Common.Services.Command.Command
    {
        private readonly IMessageQueue _messageQueue;

        private ViewUserDTO _dto;

        public CreateUserIntegrationCommand(IServiceProvider provider)
        {
            _messageQueue = provider.GetRequiredService<IMessageQueue>();
        }

        public override CommandResponse Execute()
        {
            _messageQueue.Publish<UserIntegrationMessage>("user", new UserIntegrationMessage
            {
                Id = _dto.Id,
                Birthday = _dto.Birthday
            });
            return new CommandResponse();
        }

        public override void SetData(object data)
        {
            _dto = (ViewUserDTO)data;
        }
    }
}

