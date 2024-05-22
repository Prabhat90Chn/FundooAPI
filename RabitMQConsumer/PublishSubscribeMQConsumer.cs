using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace RabbitMQConsumer
{
    public class PublishSubscribeMQConsumer
    {
        private static IModel channel;
        private static IConnection connection;
        private static ManualResetEvent manualResetEvent = new ManualResetEvent(false);

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

                
                channel.BasicConsume
                (
                    queue: "direct_queue",
                    autoAck: true,
                    consumer: consumer
                );

              
                consumer.Received += HandleMessage;

                void HandleMessage(object model, BasicDeliverEventArgs eventArgs)
                {
                    var body = eventArgs.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine("*****Rabbit MQ message****");
                    Console.WriteLine();
                    Console.WriteLine(message);
                }
                manualResetEvent.WaitOne();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                StopConsuming();
            }
        }

        public static void StopConsuming()
        {
            if (channel != null)
            {
                channel.Close();
                channel.Dispose();
            }

            if (connection != null)
            {
                connection.Close();
                connection.Dispose();
            }

            Console.WriteLine("Consumer stopped.");
        }
    }
}
