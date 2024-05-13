using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace WorkQueueConsumer
{
    public class WQConsumer
    {
        static void Main(string[] args)
        {
            var uri = new Uri("amqp://guest:guest@rabbit:5672");
            var factory = new ConnectionFactory();
             factory.Uri = uri;
            using var connection =factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "wq_queue",
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                
                int dots = message.Split(' ').Length - 1;
                Thread.Sleep(dots * 1000);

               channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };
            
            channel.BasicConsume(queue: "wq_queue",
                     autoAck: false,
                     consumer: consumer);


        }
    }
}
