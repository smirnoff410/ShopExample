using System;
namespace Common.Services.Command
{
    public class CommandResponse
    {
        public bool Success { get; set; }
        public object Response { get; set; }
    }
}

