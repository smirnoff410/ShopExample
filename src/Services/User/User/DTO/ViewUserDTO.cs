using System;
using Common.Converters;
using System.Text.Json.Serialization;

namespace User.User.DTO
{
    public class ViewUserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonConverter(typeof(DateOnlyConverter))]
        public DateOnly Birthday { get; set; }
    }
}

