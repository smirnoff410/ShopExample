using System;
using Basket.Services.DatabaseContext;
using Common.Models.User;
using Common.Services.Command;

namespace Basket.User.Integration
{
    using User.Entity;
    public class CreateUserIntegration : Command
    {
        private readonly BasketServiceDbContext _context;
        private readonly UserIntegrationMessage _message;

        public CreateUserIntegration(IServiceScope scope, UserIntegrationMessage data)
        {
            _context = scope.ServiceProvider.GetRequiredService<BasketServiceDbContext>();
            _message = data;
        }

        public override CommandResponse Execute()
        {
            var user = new User
            {
                Id = _message.Id,
                Birthday = _message.Birthday
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return new CommandResponse { Success = true };
        }
    }
}

