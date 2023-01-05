using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json");
builder.Services.AddOcelot();
builder.Services.AddCors();

var app = builder.Build();
app.UseCors(builder => builder.AllowAnyOrigin());
app.UseOcelot().Wait();

app.Run();
