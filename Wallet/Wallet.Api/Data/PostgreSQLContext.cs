using Microsoft.EntityFrameworkCore;

namespace Wallet.Api.Data
{
    public class PostgreSQLContext : DbContext
    {

        public PostgreSQLContext(DbContextOptions<PostgreSQLContext> options) : base(options)
        {
        }

        public PostgreSQLContext()
        {
        }

        public DbSet<Entities.Wallet> Wallets { get; set; }

    }
}
