using System;
using System.Text.Json.Serialization;
using Common.Converters;

namespace User.User.DTO
{
    public class CreateUserDTO
    {
        public string Name { get; set; }
        [JsonConverter(typeof(DateOnlyConverter))]
        public DateOnly Birthday { get; set; }
    }
}

