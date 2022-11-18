using System;
using Common.Converters;
using System.Text.Json.Serialization;

namespace Common.Models.User
{
    public class UserIntegrationMessage
    {
        public int Id { get; set; }
        [JsonConverter(typeof(DateOnlyConverter))]
        public DateOnly Birthday { get; set; }
    }
}

