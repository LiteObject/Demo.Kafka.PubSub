namespace Demo.Kafka.Subscriber
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Confluent.Kafka;

    class Program
    {
        static async Task Main()
        {
            var topics = new[] { "test_mh" };
            await Subscriber(topics, Console.WriteLine);

            Console.WriteLine("\n\nCompleted. Press any key to exit.");
        }
        
        static Task<Task> Subscriber(string[] topics, Action<string> message)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "test-app",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };

            using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                consumer.Subscribe(topics);
                var cts = new CancellationTokenSource();

                Console.CancelKeyPress += (_, e) =>
                {
                    e.Cancel = true;
                    cts.Cancel();
                };

                try
                {
                    while (true)
                    {
                        var consumeResult = consumer.Consume(cts.Token);
                        // var user = JsonSerializer.Deserialize<User>(consumeResult.Message.Value);
                        message(consumeResult.Message.Value);
                    }
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e);
                }
                finally
                {
                    consumer.Close();
                }
            }

            return Task.FromResult(Task.CompletedTask);
        }
    }
}
