using Catalog.Product.Integration;
using Catalog.Services.DatabaseContext;
using Common.Configure;
using Common.Models.Product;
using Common.Models.User;
using Common.Services.MessageQueue;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureSettings(builder.Configuration);
builder.Services.AddDbContext<CatalogServiceDbContext>();
builder.Services.AddSingleton<IMessageQueue, RabbitMessageQueue>();
builder.Services.AddControllers();

// Add services to the container.

var app = builder.Build();
// Configure the HTTP request pipeline.
var messageQueue = app.Services.GetRequiredService<IMessageQueue>();
messageQueue.ReceiveMessageRpc<CheckProductIntegrationMessage>("check_product_count", (scope, dto) =>
{
    var command = new CheckAvailableCountProductIntegration(scope, dto);
    return command.Execute();
});
messageQueue.Subscribe<DecreaseAvailableCountProductMessage>("change_count_product", (scope, dto) =>
{
    var command = new DecreaseAvailableCountProductIntegration(scope, dto);
    command.Execute();
});

app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.MapControllers();
app.Run();
