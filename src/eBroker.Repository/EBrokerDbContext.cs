using eBroker.Data.Entities;
using eBroker.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eBroker.Data
{
    public class EBrokerDbContext : DbContext, IEBrokerDbContext
    {
        public EBrokerDbContext(DbContextOptions options)
            : base(options)
        {

        }
        public DbSet<Equity> Equities { get; set; }
        public DbSet<Trader> Traders { get; set; }
        public DbSet<OwnedEquity> OwnedEquities { get; set; }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = "system";
                        entry.Entity.Created = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = "system";
                        entry.Entity.LastModified = DateTime.Now;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EBrokerDbContext).Assembly);

            modelBuilder.Entity<Equity>().HasData(
                new Equity { Id = 1, Name = "Axis Midcap", NAV = 76.68f, },
                new Equity { Id = 2, Name = "Mirae Asset", NAV = 33.45f },
                new Equity { Id = 3, Name = "DSP Tax Saver Fund", NAV = 84.71f }
            );

            modelBuilder.Entity<Trader>().HasData(
               new Trader { Id = 1, Name = "ABC Trader", FundValue = 10000 }
           );

            modelBuilder.Entity<OwnedEquity>()
                .HasData(
                    new OwnedEquity { EquityId = 1, TraderId = 1, Units = 10, Value = 766.88m }
                );
        }
    }
}
