using System;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Common.Services.MessageQueue
{
    public class RabbitMessageQueue : IMessageQueue
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<RabbitMessageQueue> _logger;
        private readonly IModel _channel;

        public RabbitMessageQueue(
            IServiceProvider serviceProvider,
            ILogger<RabbitMessageQueue> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;

            var factory = new ConnectionFactory
            {
                UserName = "admin",
                Password = "123",
                HostName = "",
                VirtualHost = "",
                Port = 5672
            };
            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();
        }

        public void Publish<T>(string routingKey, T data)
        {
            _channel.ExchangeDeclare(string.Empty, ExchangeType.Direct);
            var dataString = JsonSerializer.Serialize(data);
            var body = Encoding.UTF8.GetBytes(dataString);
            _channel.BasicPublish(string.Empty, routingKey, null, body);
        }

        public void Subscribe<T>(string routingKey, Action<IServiceScope, T> action)
        {
            _channel.ExchangeDeclare(string.Empty, ExchangeType.Direct);
            var queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queueName, string.Empty, routingKey);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var data = JsonSerializer.Deserialize<T>(message);
                if(data != null)
                    action(_serviceProvider.CreateScope(), data);
            };
            _channel.BasicConsume(queueName, true, consumer);
        }
    }
}

