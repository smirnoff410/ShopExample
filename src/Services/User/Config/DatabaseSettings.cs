using System;
namespace User.Config
{
    public class DatabaseSettings
    {
        public static string SectionName = "DatabaseSettings";
        public string ConnectionString { get; set; }
    }
}

