using System;
using Common.Services.Command;
using User.Services.DatabaseContext;

namespace User.User.Command
{
    public class DeleteUserCommand : Common.Services.Command.Command
    {
        private readonly UserServiceDbContext _context;
        private readonly int _id;

        public DeleteUserCommand(IServiceProvider provider, int id)
        {
            _context = provider.GetRequiredService<UserServiceDbContext>();
            _id = id;
        }

        public override CommandResponse Execute()
        {
            var user = _context.Users.Find(_id);

            _context.Users.Remove(user);
            _context.SaveChanges();

            return new CommandResponse { Success = true };
        }
    }
}

