using Adasit.Foundation.Domain.Events;

namespace Adasit.Foundation.Domain.SeedWork;

/// <summary>
/// Defines the contract for an aggregate root in domain-driven design.
/// Aggregate roots are the entry points for accessing and modifying domain objects,
/// and they are responsible for maintaining domain invariants and publishing domain events.
/// </summary>
public interface IAggregateRoot
{
    /// <summary>
    /// Gets the read-only collection of domain events that have occurred within this aggregate root.
    /// Domain events represent significant business events that have happened in the domain.
    /// </summary>
    /// <value>A read-only collection of <see cref="DomainEvent"/> objects.</value>
    /// <remarks>
    /// Events are typically raised during business operations and should be cleared after being processed
    /// (e.g., after being published to event handlers or persisted).
    /// </remarks>
    IReadOnlyCollection<DomainEvent> Events { get; }

    /// <summary>
    /// Clears all domain events from the aggregate root.
    /// This method should be called after the events have been processed to prevent duplicate processing.
    /// </summary>
    /// <remarks>
    /// Typically called after events have been published to event handlers or persisted to an event store.
    /// </remarks>
    void ClearEvents();
}
