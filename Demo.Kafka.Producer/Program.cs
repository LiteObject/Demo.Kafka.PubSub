using System;
using System.Net;
using System.Threading.Tasks;
using Confluent.Kafka;
using Demo.Kafka.ClassLibrary.Entities;
using System.Text.Json;

namespace Demo.Kafka.Producer
{
    class Program
    {
        static async Task Main()
        {
            Console.Title = "Kafka Message Producer/Publisher";

            var topics = new[] { "test_mh" };

            for (var i = 1; i <= 1000; i++)
            {
                var u = new User { Uuid = Guid.NewGuid(), Id = i, Name = $"My Name {i}", Email = $"test_{i}@test.com" };
                Console.WriteLine($"Publishing message {i}");
                await Publisher(topics[0], u);
                // await Task.Delay(1000);
            }

            Console.WriteLine("\n\nCompleted. Press any key to exit.");
        }

        static async Task Publisher(string topic, User user)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                ClientId = Dns.GetHostName()
            };

            using var producer = new ProducerBuilder<Null, string>(config).Build();
            try
            {
                var message = JsonSerializer.Serialize(user);
                await producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
        }
    }
}
