using System;
using Common.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using User.Services.DatabaseContext;
using User.User.Command;
using User.User.DTO;
using User.User.Entity;

namespace UnitTests.User
{
    public class CRUDUserTest
    {
        [Fact]
        public void CreateUserEntityTest()
        {
            var options = new DbContextOptionsBuilder<UserServiceDbContext>()
                .UseInMemoryDatabase(databaseName: "UserDatabase")
                .Options;

            var dto = new CreateUserDTO
            {
                Name = "Username",
                Birthday = DateOnly.FromDateTime(DateTime.UtcNow)
            };

            var mockOptions = new Mock<IOptions<DatabaseSettings>>();
            var mockLogger = new Mock<ILogger<UserServiceDbContext>>();
            var services = new ServiceCollection();
            services.AddScoped(x => new UserServiceDbContext(mockOptions.Object, mockLogger.Object, options));
            var provider = services.BuildServiceProvider();

            var command = new CreateUserCommand(provider, dto);
            var result = command.Execute();

            Assert.True(result.Success);

            var database = provider.GetRequiredService<UserServiceDbContext>();
            var userEntity = database.Users.FirstOrDefault();
            Assert.NotNull(userEntity);
            Assert.Equal(dto.Name, userEntity.Name);
            Assert.Equal(dto.Birthday, userEntity.Birthday);
        }

        [Fact]
        public void UpdateUserEntityTest()
        {
            var options = new DbContextOptionsBuilder<UserServiceDbContext>()
                .UseInMemoryDatabase(databaseName: "UserDatabase")
                .Options;

            var dto = new UpdateUserDTO
            {
                Name = "Username"
            };

            var mockOptions = new Mock<IOptions<DatabaseSettings>>();
            var mockLogger = new Mock<ILogger<UserServiceDbContext>>();
            var services = new ServiceCollection();
            services.AddScoped(x => new UserServiceDbContext(mockOptions.Object, mockLogger.Object, options));
            var provider = services.BuildServiceProvider();

            var userId = 1;
            var userEntity = new global::User.User.Entity.User() { Id = userId, Name = "my_name", Birthday = DateOnly.MinValue };
            var database = provider.GetRequiredService<UserServiceDbContext>();
            database.Users.Add(userEntity);

            var command = new UpdateUserCommand(provider, dto, userId);
            var result = command.Execute();

            var updateUser = database.Users.FirstOrDefault();
            Assert.NotNull(updateUser);
            Assert.Equal(dto.Name, updateUser.Name);
        }
    }
}

