using Confluent.Kafka;
using System;

namespace KafkaConsumer
{
    internal class Consumer
    {
        static void Main(string[] args)
        {
            var config = new ConsumerConfig
            {
                GroupId = "gid-consumer",
                BootstrapServers = "localhost:9092"
            };
            using (var consumer = new ConsumerBuilder<Null, string>(config).Build())
            {
                consumer.Subscribe("quickstart-events");
                while (true)
                {
                    var cr=consumer.Consume();
                    Console.WriteLine("*********Kafka Consumer Data*********");
                    Console.WriteLine();
                    Console.WriteLine(cr.Message.Value);
                }
            }
        }
    }
}
