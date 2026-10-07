using Microsoft.EntityFrameworkCore;
using Transaction.Api.Data;
using Transaction.Api.Services.Interfaces;

namespace Transaction.Api.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly PostgreSQLContext _context;

        public TransactionService(PostgreSQLContext context)
        {
            this._context = context;
        }

        public async Task<List<Entities.Transaction>> FindAll(int skip, int take)
        {
            return await this._context.Transactions
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }
    }
}
