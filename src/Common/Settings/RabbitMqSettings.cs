﻿using System;
namespace Common.Settings
{
    public class RabbitMqSettings
    {
        public static string SectionName = "RabbitMqSettings";
        public string UserName { get; set; }
        public string Password { get; set; }
        public string HostName { get; set; }
        public string VirtualHost { get; set; }
        public int Port { get; set; }
    }
}

