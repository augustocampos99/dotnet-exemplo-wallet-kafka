using System.ComponentModel.DataAnnotations;

namespace Wallet.Api.Contracts
{
    public class TransactionRequest
    {
        [Required(ErrorMessage = "Amount is required")]
        public decimal Amount { get; set; }
    }
}
