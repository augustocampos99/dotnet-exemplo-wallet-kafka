using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Transaction.Api.Enums;

namespace Transaction.Api.Entities
{
    [Table("transactions")]
    public class Transaction
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("wallet_origin_id")]
        public Guid WalletOriginId { get; set; }

        [Column("wallet_destiny_id")]
        public Guid? WalletDestinyId { get; set; }

        [Column("type")]
        public TransactionTypeEnum Type { get; set; }

        [Column("amount")]
        public decimal Amount { get; set; }

        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }

    }
}
