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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction.Api.Entities.Transaction>()
                .Property(t => t.Type)
                .HasConversion<string>()
                .HasColumnName("type");

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Entities.Transaction> Transactions { get; set; }

    }
}
