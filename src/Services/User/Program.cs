using System.Reflection;
using Common.Configure;
using Common.Middlewares;
using Common.Services.MessageQueue;
using Common.Services.Validation;
using Common.Settings;
using Microsoft.Extensions.Configuration;
using User.Services.DatabaseContext;
using User.User.DTO;
using User.User.Validation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureSettings(builder.Configuration);

// Add services to the container.

builder.Services.AddDbContext<UserServiceDbContext>();
builder.Services.AddControllers();
builder.Services.AddSingleton<IMessageQueue, RabbitMessageQueue>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapControllers();
app.UseMiddleware<ValidationMiddleware>();

app.Run();

