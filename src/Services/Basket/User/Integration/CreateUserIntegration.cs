using System;
using Basket.Services.DatabaseContext;
using Common.Models.User;
using Common.Services.Command;

namespace Basket.User.Integration
{
    using User.Entity;
    public class CreateUserIntegration : Command<UserIntegrationMessage, bool>
    {
        private readonly IServiceScope _scope;

        public CreateUserIntegration(IServiceScope scope, ILogger<CreateUserIntegration> logger)
            : base(logger)
        {
            _scope = scope;
        }

        public override bool Execute(UserIntegrationMessage data)
        {
            var db = _scope.ServiceProvider.GetRequiredService<BasketServiceDbContext>();

            var user = new User
            {
                Id = data.Id,
                Birthday = data.Birthday
            };

            db.Users.Add(user);
            db.SaveChanges();

            return true;
        }
    }
}

