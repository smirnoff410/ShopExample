using Catalog.Services.DatabaseContext;
using Common.Configure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureSettings(builder.Configuration);
builder.Services.AddDbContext<CatalogServiceDbContext>();

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.


app.Run();
