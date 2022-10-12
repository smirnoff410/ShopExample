using System;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Common.Services.MessageQueue
{
    public class RabbitMessageQueue : IMessageQueue
    {
        private readonly ILogger<RabbitMessageQueue> _logger;

        private readonly IModel _channel;
        public RabbitMessageQueue(ILogger<RabbitMessageQueue> logger)
        {
            _logger = logger;

            var factory = new ConnectionFactory
            {

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

        public void Subscribe<T>(string routingKey, Func<T, bool> action)
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
                    action(data);
            };
            _channel.BasicConsume(queueName, true, consumer);
        }
    }
}

