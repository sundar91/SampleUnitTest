using eBroker.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eBroker.Data.Configurations
{
    public class EquityConfiguration : IEntityTypeConfiguration<Equity>
    {
        public void Configure(EntityTypeBuilder<Equity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(x => x.NAV)
                .IsRequired();
        }
    }
}
