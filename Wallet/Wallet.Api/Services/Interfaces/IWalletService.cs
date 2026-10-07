using Wallet.Api.Contracts;

namespace Wallet.Api.Services.Interfaces
{
    public interface IWalletService
    {

        Task<List<Entities.Wallet>> FindAll(int skip, int take);

        Task<Entities.Wallet> Create(WalletRequest walletRequest);

    }
}
