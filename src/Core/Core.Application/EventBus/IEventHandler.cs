using Core.Domain.Primitives;

namespace Core.Application.EventBus
{
    /// <summary>
    /// Represents the event handler interface.
    /// </summary>
    /// <typeparam name="TEvent">The event type.</typeparam>
    public interface IEventHandler<in TEvent>
        where TEvent : IEvent
    {
        /// <summary>
        /// Handles the specified event.
        /// </summary>
        /// <param name="@event">The event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The completed task.</returns>
        Task Handle(TEvent @event, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Represents the event handler interface.
    /// </summary>
    public interface IEventHandler
    {
        /// <summary>
        /// Handles the specified event.
        /// </summary>
        /// <param name="@event">The event event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The completed task.</returns>
        Task Handle(IEvent @event, CancellationToken cancellationToken = default);
    }
}
