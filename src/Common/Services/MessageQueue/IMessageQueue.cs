using System;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Services.MessageQueue
{
    public interface IMessageQueue
    {
        void Publish<T>(string routingKey, T data);
        void Subscribe<T>(string routingKey, Action<IServiceScope, T> action);
    }
}

