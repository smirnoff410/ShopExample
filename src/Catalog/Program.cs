using Common.Configure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureSettings(builder.Configuration);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.


app.Run();
