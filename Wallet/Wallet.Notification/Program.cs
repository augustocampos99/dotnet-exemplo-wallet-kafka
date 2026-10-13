using Confluent.Kafka;

var config = new ConsumerConfig
{
    GroupId = "notification-consumer",
    BootstrapServers = "localhost:9092",
    AutoOffsetReset = AutoOffsetReset.Earliest
};

using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();

try
{
    consumer.Subscribe("wallet.events");

    while (true)
    {
        var message = consumer.Consume();
        Console.WriteLine(message.Message.Value);
    }
}
catch (OperationCanceledException)
{
    Console.WriteLine("Consumo cancelado.");
}
finally
{
    // Encerra o consumer e libera sua participação no grupo.
    consumer.Close();
    Console.WriteLine("Consumer encerrado.");
}
