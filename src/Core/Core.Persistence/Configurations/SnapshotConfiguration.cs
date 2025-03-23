using Core.Domain.EventStore;
using Core.Domain.Primitives;
using Core.Persistence.Converters;
using Core.Persistence.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Persistence.Configurations
{
    public abstract class SnapshotConfiguration<TAggregate> : IEntityTypeConfiguration<Snapshot<TAggregate>>
        where TAggregate : AggregateRoot
    {
        public void Configure(EntityTypeBuilder<Snapshot<TAggregate>> builder)
        {
            builder.ToTable($"{typeof(TAggregate).Name}Snapshots");

            builder.HasKey(snapshot => new { snapshot.Version, snapshot.AggregateId });

            builder
                .Property(snapshot => snapshot.Aggregate)
                .HasConversion<AggregateConverter<TAggregate>>()
                .IsRequired();

            builder
                .Property(snapshot => snapshot.AggregateId)
                .IsRequired();

            builder.Property(snapshot => snapshot.Timestamp)
                .HasConversion(
                    dt => dt.UtcDateTime,
                    dt => DateTimeOffsetHelper.ConvertToBrazilianOffset(dt))
                .IsRequired();

            builder
                .Property(snapshot => snapshot.Version)
                .IsRequired();
        }
    }
}
