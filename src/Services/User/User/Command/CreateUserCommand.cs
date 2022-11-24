using System;
using Common.Services.Command;
using User.Services.DatabaseContext;
using User.User.DTO;

namespace User.User.Command
{
    public class CreateUserCommand : Common.Services.Command.Command
    {
        private readonly UserServiceDbContext _context;
        private readonly IServiceProvider provider;
        private CreateUserDTO _dto;

        public CreateUserCommand(
            IServiceProvider provider,
            CreateUserDTO dto)
        {
            _context = provider.GetRequiredService<UserServiceDbContext>();
            this.provider = provider;
            _dto = dto;
        }

        public override CommandResponse Execute()
        {
            var user = new User.Entity.User
            {
                Name = _dto.Name,
                Birthday = _dto.Birthday
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return new CommandResponse { Success = true, Response = new ViewUserDTO { Id = user.Id, Name = user.Name, Birthday = user.Birthday } };
        }
    }
}

