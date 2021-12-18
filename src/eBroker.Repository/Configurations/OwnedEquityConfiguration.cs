using eBroker.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eBroker.Data.Configurations
{
    public class OwnedEquityConfiguration : IEntityTypeConfiguration<OwnedEquity>
    {
        public void Configure(EntityTypeBuilder<OwnedEquity> builder)
        {
            builder.HasKey(e => new { e.EquityId, e.TraderId } );

            builder.HasOne(d => d.Trader)
                .WithMany(p => p.OwnedEquities)
                .HasForeignKey(e => e.TraderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasForeignKey("FK_Owned_Equities_Traders");

            builder.HasOne(d => d.Equity)
              .WithMany(p => p.OwnedEquities)
              .HasForeignKey(d => d.EquityId)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasForeignKey("FK_Owned_Equities_Equities");
        }
    }
}
