namespace Transaction.Api.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<List<Entities.Transaction>> FindAll(int skip, int take);

        Task<List<Entities.Transaction>> FindAllByWalletId(Guid walletId, int skip, int take);

    }
}
