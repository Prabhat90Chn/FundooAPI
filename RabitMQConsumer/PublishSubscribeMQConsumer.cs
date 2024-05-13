using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Data.Common;
using System;
using System.Text;
using System.Threading.Channels;

namespace DirectConsumer
{
    public class PublishSubscribeMQConsumer
    {
        static private IModel channel;
        static private IConnection connection;
        static void Main(string[] args)
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = "localhost",
                    Port = 5672,
                    UserName = "guest",
                    Password = "guest"
                };

                connection = factory.CreateConnection();
                channel = connection.CreateModel();

                channel.QueueDeclare
                   (
                    queue: "direct_queue",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                    );

                var consumer = new EventingBasicConsumer(channel);

                /*consumer.Received += (model, eventArgs) =>
                {
                    var body = eventArgs.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                };*/

                consumer.Received += HandleMessage;

                void HandleMessage(object model, BasicDeliverEventArgs eventArgs)
                {
                    var body = eventArgs.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                }

                channel.BasicConsume
                   (
                   queue: "direct_queue",
                   autoAck: true,
                   consumer: consumer
                   );

            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void StopConsuming()
        {
            channel.Close();
            connection.Close();
            Console.WriteLine("Consumer stopped.");
        }
    }
}
