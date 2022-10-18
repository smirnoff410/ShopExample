using System;
using Microsoft.Extensions.Logging;

namespace Common.Services.Command
{
    public abstract class Command<TRequest, TResponse>
    {
        protected readonly ILogger<Command<TRequest, TResponse>> _logger;

        public Command(ILogger<Command<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public abstract TResponse Execute(TRequest data);
    }
}

