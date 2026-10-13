using Confluent.Kafka;
using System.Text.Json;
using Transaction.Api.Data;
using Transaction.Api.Entities;

namespace Transaction.Api.Consumers
{
    public class ConsumerTransactionService : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _scopeFactory;

        public ConsumerTransactionService(
            IConfiguration configuration,
            IServiceScopeFactory scopeFactory)
        {
            _configuration = configuration;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string kafkaHost =  _configuration.GetValue<string>("KafkaConfiguration:Host")!;
            string walletTopic = _configuration.GetValue<string>("KafkaConfiguration:Topics:WALLET")!;

            var config = new ConsumerConfig
            {
                GroupId = "transaction-consumer",
                BootstrapServers = kafkaHost,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();

            consumer.Subscribe(walletTopic);

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var cr = consumer.Consume(stoppingToken);

                    Console.WriteLine($"Consumed message '{cr.Value}' at: '{cr.TopicPartitionOffset}'.");

                    var transaction =JsonSerializer.Deserialize<Transaction.Api.Entities.Transaction>(cr.Value);

                    if (transaction is null)
                        continue;

                    using var scope = _scopeFactory.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<PostgreSQLContext>();

                    transaction.CreatedAt = DateTime.Now;
                    await context.Transactions.AddAsync(transaction, stoppingToken);
                    await context.SaveChangesAsync(stoppingToken);

                    consumer.Commit(cr);
                }
            }
            catch (OperationCanceledException)
            {
                // Application is shutting down.
            }
            finally
            {
                consumer.Close();
            }
        }

    }
}
