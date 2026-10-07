using Wallet.Api.Enums;

namespace Wallet.Api.Contracts
{
    public class TransactionQueue
    {
        public Guid Id { get; set; }

        public Guid WalletId { get; set; }

        public TransactionTypeEnum Type { get; set; }

        public decimal Amount { get; set; }
    }
}
