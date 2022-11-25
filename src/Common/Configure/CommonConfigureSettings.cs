using System;
using System.Reflection;
using Common.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Configure
{
    public static class CommonConfigureSettings
    {
        public static void ConfigureSettings(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.Configure<DatabaseSettings>(configuration.GetSection(DatabaseSettings.SectionName));
            services.Configure<RabbitMqSettings>(configuration.GetSection(RabbitMqSettings.SectionName));
        }
    }
}

