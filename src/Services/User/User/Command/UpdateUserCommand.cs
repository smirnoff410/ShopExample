using System;
using Common.Services.Command;
using Microsoft.EntityFrameworkCore;
using User.Services.DatabaseContext;
using User.User.DTO;

namespace User.User.Command
{
    public class UpdateUserCommand : Common.Services.Command.Command
    {
        private readonly UpdateUserDTO _dto;
        private readonly int _id;
        private readonly UserServiceDbContext _context;

        public UpdateUserCommand(
            IServiceProvider provider,
            UpdateUserDTO dto,
            int id)
        {
            _context = provider.GetRequiredService<UserServiceDbContext>();
            _dto = dto;
            _id = id;
        }

        public override CommandResponse Execute()
        {
            var user = _context.Users.Find(_id);
            user.Name = _dto.Name;

            _context.SaveChanges();

            return new CommandResponse { Success = true };
        }
    }
}

