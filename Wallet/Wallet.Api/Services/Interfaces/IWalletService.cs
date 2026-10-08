using Wallet.Api.Contracts;

namespace Wallet.Api.Services.Interfaces
{
    public interface IWalletService
    {

        Task<List<Entities.Wallet>> FindAll(int skip, int take);

        Task<Entities.Wallet?> FindById(Guid id);

        Task<Entities.Wallet> Create(WalletRequest walletRequest);

        Task<Entities.Wallet> Deposit(Guid id, TransactionRequest transaction);

        Task<Entities.Wallet> Withdraw(Guid id, TransactionRequest transaction);

        Task<Entities.Wallet> Transfer(Guid originId, TransferRequest transfer);

    }
}
