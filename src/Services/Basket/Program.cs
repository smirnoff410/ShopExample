using System.Reflection;
using Basket.Product.Integration;
using Basket.Services.DatabaseContext;
using Basket.User.Integration;
using Common.Configure;
using Common.Models.Product;
using Common.Models.User;
using Common.Services.MessageQueue;
using Common.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureSettings(builder.Configuration);

builder.Services.AddDbContext<BasketServiceDbContext>();
builder.Services.AddSingleton<IMessageQueue, RabbitMessageQueue>();
builder.Services.AddControllers();

// Add services to the container.

var app = builder.Build();

var messageQueue = app.Services.GetRequiredService<IMessageQueue>();
messageQueue.Subscribe<UserIntegrationMessage>("user", (scope, dto) =>
{
    var command = new CreateUserIntegration(scope, dto);
    command.Execute();
});
messageQueue.Subscribe<ProductIntegrationMessage>("product", (scope, dto) =>
{
    var command = new CreateProductIntegration(scope, dto);
    command.Execute();
});

// Configure the HTTP request pipeline.
app.MapControllers();
app.Run();
