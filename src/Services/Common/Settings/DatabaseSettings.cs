using System;
namespace Common.Settings
{
    public class DatabaseSettings
    {
        public static string SectionName = "DatabaseSettings";
        public string ConnectionString { get; set; }
    }
}

