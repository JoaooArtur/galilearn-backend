using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Core.Domain.EventStore;
using Core.Domain.Primitives;
using Core.Persistence.Converters;
using Core.Persistence.Helpers;

namespace Core.Persistence.Configurations
{
    public abstract class StoreEventConfiguration<TAggregate> : IEntityTypeConfiguration<StoreEvent<TAggregate>>
        where TAggregate : AggregateRoot
    {
        public void Configure(EntityTypeBuilder<StoreEvent<TAggregate>> builder)
        {
            builder.ToTable($"{typeof(TAggregate).Name}StoreEvents");

            builder.HasKey(@event => new { @event.Version, @event.AggregateId });

            builder
                .Property(@event => @event.AggregateId)
                .IsRequired();

            builder
                .Property(@event => @event.Event)
                .HasConversion<EventConverter>()
                .IsRequired();

            builder
                .Property(@event => @event.EventType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();

            builder
                .Property(@event => @event.Timestamp)
                .HasConversion(
                    dt => dt.UtcDateTime,
                    dt => DateTimeOffsetHelper.ConvertToBrazilianOffset(dt))
                .IsRequired();

            builder
                .Property(@event => @event.Version)
                .IsRequired();
        }
    }
}
