using System.ComponentModel.DataAnnotations;

namespace Wallet.Api.Contracts
{
    public class WalletRequest
    {
        [Required(ErrorMessage = "Balance is required")]
        public decimal Balance { get; set; }
    }
}
