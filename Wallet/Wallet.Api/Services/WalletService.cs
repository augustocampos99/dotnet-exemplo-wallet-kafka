using Microsoft.EntityFrameworkCore;
using Wallet.Api.Contracts;
using Wallet.Api.Data;
using Wallet.Api.Services.Interfaces;

namespace Wallet.Api.Services
{
    public class WalletService : IWalletService
    {
        private readonly PostgreSQLContext _context;

        public WalletService(PostgreSQLContext context)
        {
            this._context = context;            
        }

        public async Task<List<Entities.Wallet>> FindAll(int skip, int take)
        {
            return await this._context.Wallets
                .Skip(skip)
                .Take(take)
                .ToListAsync();
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
    }
}
