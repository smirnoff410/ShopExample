﻿using Common.Configure;
using Common.Settings;
using User.Services.DatabaseContext;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureSettings(builder.Configuration);

// Add services to the container.

builder.Services.AddDbContext<UserServiceDbContext>();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapControllers();

app.Run();

