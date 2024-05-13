using FundooAPI.Exceptions;
using FundooAPI.RabitMQ.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;

namespace FundooAPI.RabitMQ.Service
{
    public class PublishSubscribeMQProducer : IPublishSubscribeMQProducer
    {
        private readonly ILogger<PublishSubscribeMQProducer> _logger;
        private  string _hostName;
        private  int _port;
        private  string _userName;
        private string _password;


        public PublishSubscribeMQProducer(ILogger<PublishSubscribeMQProducer> logger, IConfiguration configuration)
        {
            _logger = logger;
            _hostName = configuration["RabbitMQ:HostName"];
            _port = configuration.GetValue<int>("RabbitMQ:Port");
            _userName = configuration["RabbitMQ:UserName"];
            _password = configuration["RabbitMQ:Password"];

        }
        public void Publish<T>(T message)
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _hostName,
                    Port = _port,
                    UserName = _userName,
                    Password = _password
                };

                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                channel.ExchangeDeclare(
                    exchange: "amq.direct",
                    //exchange: "direct_exchange",
                    type: ExchangeType.Direct,
                    durable: true,
                    autoDelete: false
                    );

                var queueName = "direct_queue";
                channel.QueueDeclare(
                    queue: queueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                var json = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(json);

                var bindingKey = "userInfo";

                channel.QueueBind(
                    queue: queueName,
                    exchange: "amq.direct",
                    routingKey: bindingKey
                );


                channel.BasicPublish(
                    exchange: "amq.direct",
                    routingKey: bindingKey,
                    basicProperties: null,
                    body: body);

                _logger.LogInformation("Message published successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while publishing the message.");
                throw new RabbitMQProducerException(ex.Message,ex);
            }
        }
    }
}

