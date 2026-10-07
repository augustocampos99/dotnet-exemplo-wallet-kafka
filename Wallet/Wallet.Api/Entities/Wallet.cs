using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Transactions;

namespace Wallet.Api.Entities
{
    [Table("wallets")]
    public class Wallet
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("balance")]
        public decimal Balance { get; set; }

        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at", TypeName = "timestamp without time zone")]
        public DateTime UpdatedAt { get; set; }

    }
}
