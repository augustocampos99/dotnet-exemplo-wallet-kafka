using Confluent.Kafka;

var config = new ConsumerConfig
{
    GroupId = "notification-comsumer",
    BootstrapServers = "localhost:9092",
    AutoOffsetReset = AutoOffsetReset.Earliest
};

using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
{
    consumer.Subscribe("wallet.events");

    try
    {
        while (true)
        {
            var cr = consumer.Consume();
            Console.WriteLine($"Consumed message '{cr.Value}' at: '{cr.TopicPartitionOffset}'.");
        }
    }
    catch (OperationCanceledException)
    {
        consumer.Close();
    }
}