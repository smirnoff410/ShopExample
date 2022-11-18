using System;
using Common.Services.Command;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Services.MessageQueue
{
    public interface IMessageQueue
    {
        void Publish<T>(string routingKey, T data);
        void Subscribe<T>(string routingKey, Action<IServiceScope, T> action);

        TResponse SendMessageRpc<TRequest, TResponse>(string routingKey, TRequest request);
        void ReceiveMessageRpc<TRequest>(string routingKey, Func<IServiceScope, TRequest, CommandResponse> action);
    }
}

