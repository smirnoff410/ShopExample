using System;
using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using Common.Services.Command;
using Common.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
            IOptions<RabbitMqSettings> options,
            ILogger<RabbitMessageQueue> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;

            var factory = new ConnectionFactory
            {
                UserName = options.Value.UserName,
                Password = options.Value.Password,
                HostName = options.Value.HostName,
                VirtualHost = options.Value.VirtualHost,
                Port = options.Value.Port
            };
            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();
        }

        public void Publish<T>(string routingKey, T data)
        {
            var dataString = JsonSerializer.Serialize(data);
            var body = Encoding.UTF8.GetBytes(dataString);
            _channel.BasicPublish(string.Empty, routingKey, null, body);
        }

        public void ReceiveMessageRpc<TRequest>(string routingKey, Func<IServiceScope, TRequest, CommandResponse> action)
        {
            _channel.QueueDeclare(
                queue: routingKey,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            _channel.BasicQos(0, 1, false);
            var consumer = new EventingBasicConsumer(_channel);
            _channel.BasicConsume(
                queue: routingKey,
                autoAck: false,
                consumer: consumer);

            consumer.Received += (model, ea) =>
            {
                var response = new CommandResponse();

                var body = ea.Body.ToArray();
                var props = ea.BasicProperties;
                var replyProps = _channel.CreateBasicProperties();
                replyProps.CorrelationId = props.CorrelationId;

                try
                {
                    var message = Encoding.UTF8.GetString(body);
                    var data = JsonSerializer.Deserialize<TRequest>(message);
                    if (data != null)
                        response = action(_serviceProvider.CreateScope(), data);
                }
                catch (Exception e)
                {
                    response.Success = false;
                    _logger.LogError(e, e.Message);
                }
                finally
                {
                    var responseString = JsonSerializer.Serialize(response.Response);
                    var responseBytes = Encoding.UTF8.GetBytes(responseString);
                    _channel.BasicPublish(
                        exchange: "",
                        routingKey: props.ReplyTo,
                        basicProperties: replyProps,
                        body: responseBytes);
                    _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
            };
        }

        public TResponse SendMessageRpc<TRequest, TResponse>(string routingKey, TRequest request)
        {
            var respQueue = new BlockingCollection<TResponse>();
            var props = _channel.CreateBasicProperties();
            var correlationId = Guid.NewGuid().ToString();
            props.CorrelationId = correlationId;
            var replyQueueName = _channel.QueueDeclare().QueueName;
            props.ReplyTo = replyQueueName;

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var response = Encoding.UTF8.GetString(body);
                var data = JsonSerializer.Deserialize<TResponse>(response);
                if (ea.BasicProperties.CorrelationId == correlationId)
                {
                    respQueue.Add(data);
                }
            };
            _channel.BasicConsume(
                consumer: consumer,
                queue: props.ReplyTo,
                autoAck: true);

            var message = JsonSerializer.Serialize(request);
            var messageBytes = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(
                exchange: "",
                routingKey: routingKey,
                basicProperties: props,
                body: messageBytes);

            return respQueue.Take();
        }

        public void Subscribe<T>(string routingKey, Action<IServiceScope, T> action)
        {
            var queueName = _channel.QueueDeclare(routingKey);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var data = JsonSerializer.Deserialize<T>(message);
                if(data != null)
                    action(_serviceProvider.CreateScope(), data);
            };
            _channel.BasicConsume(routingKey, true, consumer);
        }
    }
}

