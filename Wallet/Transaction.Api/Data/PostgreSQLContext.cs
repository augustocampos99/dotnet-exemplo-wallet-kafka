using Microsoft.EntityFrameworkCore;

namespace Transaction.Api.Data
{
    public class PostgreSQLContext : DbContext
    {

        public PostgreSQLContext(DbContextOptions<PostgreSQLContext> options) : base(options)
        {            
        }

        public PostgreSQLContext()
        {            
        }

        public DbSet<Entities.Transaction> Transactions { get; set; }

    }
}
