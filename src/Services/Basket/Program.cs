using Basket.User.Integration;
using Common.Configure;
using Common.Models.User;
using Common.Services.MessageQueue;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureSettings(builder.Configuration);

// Add services to the container.

var app = builder.Build();

var messageQueue = app.Services.GetRequiredService<IMessageQueue>();
messageQueue.Subscribe<UserIntegrationMessage>("user", (scope, dto) =>
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<CreateUserIntegration>>();
    var command = new CreateUserIntegration(scope, logger);
    command.Execute(dto);
});

// Configure the HTTP request pipeline.

app.Run();
