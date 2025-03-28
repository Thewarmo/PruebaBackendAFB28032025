using Microsoft.EntityFrameworkCore;
using CargoPayWebApi.Models;

namespace CargoPayWebApi.Data
{
    public class CargoPayDbContext : DbContext
    {
        public CargoPayDbContext(DbContextOptions<CargoPayDbContext> options)
            : base(options)
        {
        }

        public DbSet<Card> Cards { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Card>()
                .HasIndex(c => c.CardNumber)
                .IsUnique();

            modelBuilder.Entity<Card>()
                .Property(c => c.Balance)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Card)
                .WithMany(c => c.Transactions)
                .HasForeignKey(t => t.CardId);

            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Transaction>()
                .Property(t => t.Fee)
                .HasPrecision(18, 2);
        }
    }
}