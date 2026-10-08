using System.ComponentModel.DataAnnotations;

namespace Wallet.Api.Contracts
{
    public class TransferRequest
    {
        [Required(ErrorMessage = "WalletDestinyId is required")]
        public Guid WalletDestinyId { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        public decimal Amount { get; set; }
    }
}
