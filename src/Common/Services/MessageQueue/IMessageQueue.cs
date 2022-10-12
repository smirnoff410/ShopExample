using System;
namespace Common.Services.MessageQueue
{
    public interface IMessageQueue
    {
        void Publish<T>(string routingKey, T data);
        void Subscribe<T>(string routingKey, Func<T, bool> action);
    }
}

