using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Wallet.Api.Contracts;
using Wallet.Api.Data;
using Wallet.Api.Entities;
using Wallet.Api.Services.Interfaces;

namespace Wallet.Api.Services
{
    public class WalletService : IWalletService
    {
        private readonly PostgreSQLContext _context;
        private readonly IConfiguration _configuration;

        public WalletService(PostgreSQLContext context, IConfiguration configuration)
        {
            this._context = context;            
            this._configuration = configuration;
        }

        public async Task<List<Entities.Wallet>> FindAll(int skip, int take)
        {
            return await this._context.Wallets
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<Entities.Wallet?> FindById(Guid id)
        {
            var wallet = await this._context.Wallets
                .Where(e => e.Id == id)
                .FirstOrDefaultAsync();

            return wallet;
        }

        public async Task<Entities.Wallet> Create(WalletRequest walletRequest)
        {
            if (walletRequest.Balance < 0)
            {
                throw new BadRequestException("The balance must be greater than or equal zero");
            }

            var wallet = new Entities.Wallet()
            {
                Balance = walletRequest.Balance,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            this._context.Wallets.Add(wallet);
            await this._context.SaveChangesAsync();

            return wallet;
        }

        public async Task<Entities.Wallet> Deposit(Guid id, TransactionRequest transactionRequest)
        {
            var wallet = await this._context.Wallets
                .Where(e => e.Id == id)
                .FirstOrDefaultAsync();

            if (wallet == null)
            {
                throw new BadRequestException("Wallet not found");
            }
            if (transactionRequest.Amount <= 0)
            {
                throw new BadRequestException("The amount must be greater than zero");
            }

            wallet.Balance += transactionRequest.Amount;
            wallet.UpdatedAt = DateTime.Now;
            await this._context.SaveChangesAsync();

            var transaction = new TransactionQueue()
            {
                Id = Guid.NewGuid(),
                WalletOriginId = wallet.Id,
                WalletDestinyId = null,
                Amount = transactionRequest.Amount,
                Type = Enums.TransactionTypeEnum.DEPOSIT
            };
            await this.KafkaProducer(JsonSerializer.Serialize(transaction));

            return wallet;
        }

        public async Task<Entities.Wallet> Withdraw(Guid id, TransactionRequest transactionRequest)
        {
            var wallet = await this._context.Wallets
                .Where(e => e.Id == id)
                .FirstOrDefaultAsync();

            if (wallet == null)
            {
                throw new BadRequestException("Wallet not found");
            }
            if (transactionRequest.Amount <= 0)
            {
                throw new BadRequestException("The amount must be greater than zero");
            }
            if (transactionRequest.Amount > wallet.Balance)
            {
                throw new BadRequestException("You do not have sufficient funds");
            }

            wallet.Balance -= transactionRequest.Amount;
            wallet.UpdatedAt = DateTime.Now;
            await this._context.SaveChangesAsync();

            var transaction = new TransactionQueue()
            {
                Id = Guid.NewGuid(),
                WalletOriginId = wallet.Id,
                WalletDestinyId = null,
                Amount = transactionRequest.Amount,
                Type = Enums.TransactionTypeEnum.WITHDRAW
            };
            await this.KafkaProducer(JsonSerializer.Serialize(transaction));

            return wallet;
        }

        public async Task<Entities.Wallet> Transfer(Guid originId, TransferRequest transferRequest)
        {
            var walletOrigin = await this._context.Wallets
                .Where(e => e.Id == originId)
                .FirstOrDefaultAsync();

            var walletDestiny = await this._context.Wallets
                .Where(e => e.Id == transferRequest.WalletDestinyId)
                .FirstOrDefaultAsync();

            if (walletOrigin == null || walletDestiny == null)
            {
                throw new BadRequestException("Wallet not found");
            }
            if (transferRequest.Amount <= 0)
            {
                throw new BadRequestException("The amount must be greater than zero");
            }
            if (transferRequest.Amount > walletOrigin.Balance)
            {
                throw new BadRequestException("You do not have sufficient funds");
            }

            walletOrigin.Balance -= transferRequest.Amount;
            walletOrigin.UpdatedAt = DateTime.Now;

            walletDestiny.Balance += transferRequest.Amount;
            walletDestiny.UpdatedAt = DateTime.Now;

            await this._context.SaveChangesAsync();

            var transaction = new TransactionQueue()
            {
                Id = Guid.NewGuid(),
                WalletOriginId = walletOrigin.Id,
                WalletDestinyId = transferRequest.WalletDestinyId,
                Amount = transferRequest.Amount,
                Type = Enums.TransactionTypeEnum.TRANSFER
            };
            await this.KafkaProducer(JsonSerializer.Serialize(transaction));

            return walletOrigin;
        }

        private async Task KafkaProducer(string message)
        {
            string kafkaHost = _configuration.GetValue<string>("KafkaConfiguration:Host")!;
            string walletTopic = _configuration.GetValue<string>("KafkaConfiguration:Topics:WALLET")!;

            var config = new ProducerConfig { BootstrapServers = kafkaHost };

            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                    var deliveryResult = await producer.ProduceAsync(walletTopic, new Message<Null, string> { Value = message });
                    Console.WriteLine($"Delivered '{deliveryResult.Value}' to '{deliveryResult.TopicPartitionOffset}'");
                }
                catch (ProduceException<Null, string> e)
                {
                    Console.WriteLine($"Delivery failed: {e.Error.Reason}");
                }
            }
        }
    }
}
