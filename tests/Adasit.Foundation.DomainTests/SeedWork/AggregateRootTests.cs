using Adasit.Foundation.Domain;
using Adasit.Foundation.Domain.Events;
using Adasit.Foundation.Domain.SeedWork;
using FluentAssertions;

namespace Adasit.Foundation.DomainTests.SeedWork;

public class AggregateRootTests
{
    #region Constructor Tests

    [Fact]
    public void Constructor_ShouldInitializeWithEmptyEventCollection()
    {
        // Act
        var aggregate = new TestAggregateRoot();

        // Assert
        aggregate.Events.Should().NotBeNull();
        aggregate.Events.Should().BeEmpty();
    }

    [Fact]
    public void Constructor_ShouldInheritFromEntity()
    {
        // Act
        var aggregate = new TestAggregateRoot();

        // Assert
        aggregate.Should().BeAssignableTo<Entity<TestEntityId>>();
    }

    [Fact]
    public void Constructor_ShouldImplementIAggregateRoot()
    {
        // Act
        var aggregate = new TestAggregateRoot();

        // Assert
        aggregate.Should().BeAssignableTo<IAggregateRoot>();
    }

    [Fact]
    public void Constructor_ShouldInitializeIdFromEntity()
    {
        // Act
        var aggregate = new TestAggregateRoot();

        // Assert
        aggregate.Id.Should().NotBeNull();
        aggregate.Id.Value.Should().NotBe(Guid.Empty);
    }

    #endregion

    #region RaiseDomainEvent Tests

    [Fact]
    public void RaiseDomainEvent_WhenCalled_ShouldAddEventToCollection()
    {
        // Arrange
        var aggregate = new TestAggregateRoot();
        var domainEvent = new TestDomainEvent
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid()
        };

        // Act
        aggregate.RaiseTestEvent(domainEvent);

        // Assert
        aggregate.Events.Should().HaveCount(1);
        aggregate.Events.Should().Contain(domainEvent);
    }

    [Fact]
    public void RaiseDomainEvent_WhenCalledMultipleTimes_ShouldAddAllEvents()
    {
        // Arrange
        var aggregate = new TestAggregateRoot();
        var event1 = new TestDomainEvent { UserId = Guid.NewGuid() };
        var event2 = new TestDomainEvent { UserId = Guid.NewGuid() };
        var event3 = new TestDomainEvent { UserId = Guid.NewGuid() };

        // Act
        aggregate.RaiseTestEvent(event1);
        aggregate.RaiseTestEvent(event2);
        aggregate.RaiseTestEvent(event3);

        // Assert
        aggregate.Events.Should().HaveCount(3);
        aggregate.Events.Should().ContainInOrder(event1, event2, event3);
    }

    [Fact]
    public void RaiseDomainEvent_WhenRaisingSameEventTwice_ShouldAddOnlyOne()
    {
        // Arrange
        var aggregate = new TestAggregateRoot();
        var domainEvent = new TestDomainEvent { UserId = Guid.NewGuid() };

        // Act
        aggregate.RaiseTestEvent(domainEvent);
        aggregate.RaiseTestEvent(domainEvent);

        // Assert
        aggregate.Events.Should().HaveCount(1);
    }

    [Fact]
    public void RaiseDomainEvent_WhenRaisingDifferentEventTypes_ShouldAddAll()
    {
        // Arrange
        var aggregate = new TestAggregateRoot();
        var event1 = new TestDomainEvent { UserId = Guid.NewGuid() };
        var event2 = new AnotherTestDomainEvent { UserId = Guid.NewGuid() };

        // Act
        aggregate.RaiseTestEvent(event1);
        aggregate.RaiseTestEvent(event2);

        // Assert
        aggregate.Events.Should().HaveCount(2);
        aggregate.Events.Should().Contain(event1);
        aggregate.Events.Should().Contain(event2);
    }

    #endregion

    #region Events Property Tests

    [Fact]
    public void Events_ShouldReturnReadOnlyCollection()
    {
        // Arrange
        var aggregate = new TestAggregateRoot();
        var domainEvent = new TestDomainEvent { UserId = Guid.NewGuid() };
        aggregate.RaiseTestEvent(domainEvent);

        // Act
        var events = aggregate.Events;

        // Assert
        events.Should().BeAssignableTo<IReadOnlyCollection<DomainEvent>>();
    }

    [Fact]
    public void Events_WhenNoEventsRaised_ShouldReturnEmptyCollection()
    {
        // Arrange
        var aggregate = new TestAggregateRoot();

        // Act
        var events = aggregate.Events;

        // Assert
        events.Should().BeEmpty();
    }

    [Fact]
    public void Events_AfterRaisingEvents_ShouldReflectChanges()
    {
        // Arrange
        var aggregate = new TestAggregateRoot();
        var event1 = new TestDomainEvent { UserId = Guid.NewGuid() };

        // Act
        aggregate.RaiseTestEvent(event1);
        var eventsAfterFirst = aggregate.Events.Count;

        var event2 = new TestDomainEvent { UserId = Guid.NewGuid() };
        aggregate.RaiseTestEvent(event2);
        var eventsAfterSecond = aggregate.Events.Count;

        // Assert
        eventsAfterFirst.Should().Be(1);
        eventsAfterSecond.Should().Be(2);
    }

    [Fact]
    public void Events_ShouldCreateNewCollectionOnEachAccess()
    {
        // Arrange
        var aggregate = new TestAggregateRoot();
        var domainEvent = new TestDomainEvent { UserId = Guid.NewGuid() };

        // Act
        var events1 = aggregate.Events;
        aggregate.RaiseTestEvent(domainEvent);
        var events2 = aggregate.Events;

        // Assert
        events1.Should().NotBeSameAs(events2);
        events1.Should().HaveCount(0);
        events2.Should().HaveCount(1);
    }

    #endregion

    #region ClearEvents Tests

    [Fact]
    public void ClearEvents_WhenEventsExist_ShouldRemoveAllEvents()
    {
        // Arrange
        var aggregate = new TestAggregateRoot();
        aggregate.RaiseTestEvent(new TestDomainEvent { UserId = Guid.NewGuid() });
        aggregate.RaiseTestEvent(new TestDomainEvent { UserId = Guid.NewGuid() });
        aggregate.RaiseTestEvent(new TestDomainEvent { UserId = Guid.NewGuid() });

        // Act
        aggregate.ClearEvents();

        // Assert
        aggregate.Events.Should().BeEmpty();
    }

    [Fact]
    public void ClearEvents_WhenNoEvents_ShouldNotThrow()
    {
        // Arrange
        var aggregate = new TestAggregateRoot();

        // Act
        var act = () => aggregate.ClearEvents();

        // Assert
        act.Should().NotThrow();
        aggregate.Events.Should().BeEmpty();
    }

    [Fact]
    public void ClearEvents_AfterClearing_ShouldAllowNewEventsToBeRaised()
    {
        // Arrange
        var aggregate = new TestAggregateRoot();
        aggregate.RaiseTestEvent(new TestDomainEvent { UserId = Guid.NewGuid() });
        aggregate.ClearEvents();

        var newEvent = new TestDomainEvent { UserId = Guid.NewGuid() };

        // Act
        aggregate.RaiseTestEvent(newEvent);

        // Assert
        aggregate.Events.Should().HaveCount(1);
        aggregate.Events.Should().Contain(newEvent);
    }

    [Fact]
    public void ClearEvents_WhenCalledMultipleTimes_ShouldNotThrow()
    {
        // Arrange
        var aggregate = new TestAggregateRoot();
        aggregate.RaiseTestEvent(new TestDomainEvent { UserId = Guid.NewGuid() });

        // Act
        var act = () =>
        {
            aggregate.ClearEvents();
            aggregate.ClearEvents();
            aggregate.ClearEvents();
        };

        // Assert
        act.Should().NotThrow();
        aggregate.Events.Should().BeEmpty();
    }

    #endregion

    #region Integration Tests

    [Fact]
    public void AggregateRoot_TypicalWorkflow_ShouldWorkCorrectly()
    {
        // Arrange
        var aggregate = new TestAggregateRoot();

        // Act - Raise some events
        aggregate.RaiseTestEvent(new TestDomainEvent { UserId = Guid.NewGuid() });
        aggregate.RaiseTestEvent(new TestDomainEvent { UserId = Guid.NewGuid() });

        // Assert - Events should be collected
        aggregate.Events.Should().HaveCount(2);

        // Act - Process events (simulated) and clear
        var eventsToProcess = aggregate.Events.ToList();
        aggregate.ClearEvents();

        // Assert - Events cleared but we have the copy
        aggregate.Events.Should().BeEmpty();
        eventsToProcess.Should().HaveCount(2);

        // Act - Continue with new events
        aggregate.RaiseTestEvent(new TestDomainEvent { UserId = Guid.NewGuid() });

        // Assert - New events tracked
        aggregate.Events.Should().HaveCount(1);
    }

    [Fact]
    public void AggregateRoot_MultipleAggregates_ShouldHaveIndependentEventCollections()
    {
        // Arrange
        var aggregate1 = new TestAggregateRoot();
        var aggregate2 = new TestAggregateRoot();

        var event1 = new TestDomainEvent { UserId = Guid.NewGuid() };
        var event2 = new TestDomainEvent { UserId = Guid.NewGuid() };

        // Act
        aggregate1.RaiseTestEvent(event1);
        aggregate2.RaiseTestEvent(event2);

        // Assert
        aggregate1.Events.Should().HaveCount(1);
        aggregate1.Events.Should().Contain(event1);
        aggregate1.Events.Should().NotContain(event2);

        aggregate2.Events.Should().HaveCount(1);
        aggregate2.Events.Should().Contain(event2);
        aggregate2.Events.Should().NotContain(event1);
    }

    [Fact]
    public void AggregateRoot_AsIAggregateRoot_ShouldExposeCorrectInterface()
    {
        // Arrange
        IAggregateRoot aggregate = new TestAggregateRoot();
        var domainEvent = new TestDomainEvent { UserId = Guid.NewGuid() };

        // Act
        ((TestAggregateRoot)aggregate).RaiseTestEvent(domainEvent);

        // Assert
        aggregate.Events.Should().HaveCount(1);
        aggregate.Events.Should().Contain(domainEvent);

        // Act
        aggregate.ClearEvents();

        // Assert
        aggregate.Events.Should().BeEmpty();
    }

    #endregion

    #region Event Ordering Tests

    [Fact]
    public void Events_ShouldMaintainInsertionOrder()
    {
        // Arrange
        var aggregate = new TestAggregateRoot();
        var event1 = new TestDomainEvent { EventId = Guid.NewGuid(), UserId = Guid.NewGuid() };
        var event2 = new TestDomainEvent { EventId = Guid.NewGuid(), UserId = Guid.NewGuid() };
        var event3 = new TestDomainEvent { EventId = Guid.NewGuid(), UserId = Guid.NewGuid() };

        // Act
        aggregate.RaiseTestEvent(event1);
        aggregate.RaiseTestEvent(event2);
        aggregate.RaiseTestEvent(event3);

        // Assert
        var eventList = aggregate.Events.ToList();
        eventList[0].Should().Be(event1);
        eventList[1].Should().Be(event2);
        eventList[2].Should().Be(event3);
    }

    #endregion

    #region Inheritance Tests

    [Fact]
    public void DerivedAggregateRoot_ShouldInheritEventBehavior()
    {
        // Arrange
        var derived = new DerivedTestAggregateRoot();
        var domainEvent = new TestDomainEvent { UserId = Guid.NewGuid() };

        // Act
        derived.RaiseTestEvent(domainEvent);

        // Assert
        derived.Events.Should().HaveCount(1);
        derived.Events.Should().Contain(domainEvent);
    }

    [Fact]
    public void DerivedAggregateRoot_ShouldBeAbleToOverrideProtectedMethods()
    {
        // Arrange
        var derived = new DerivedTestAggregateRoot();

        // Act
        derived.PerformBusinessOperation();

        // Assert
        derived.Events.Should().HaveCount(1);
        derived.Events.First().Should().BeOfType<TestDomainEvent>();
    }

    // Fix for CS8872: Make Equals virtual to allow overriding in non-sealed record
    private record TestEntityId : IId<TestEntityId>, IEquatable<TestEntityId>
    {
        public Guid Value { get; }

        private TestEntityId(Guid value)
        {
            Value = value;
        }

        public static TestEntityId New() => new(Guid.NewGuid());

        public static TestEntityId Load(Guid value) => new(value);

        public virtual bool Equals(TestEntityId? other)
        {
            if (other is null) return false;
            return Value.Equals(other.Value);
        }

        public override int GetHashCode() => Value.GetHashCode();
    }

    // Test aggregate root implementation
    private class TestAggregateRoot : AggregateRoot<TestEntityId>
    {
        // Expose protected method for testing
        public void RaiseTestEvent(DomainEvent @event)
        {
            RaiseDomainEvent(@event);
        }
    }

    // Derived aggregate root for inheritance testing
    private class DerivedTestAggregateRoot : TestAggregateRoot
    {
        public void PerformBusinessOperation()
        {
            // Simulate a business operation that raises an event
            var @event = new TestDomainEvent
            {
                UserId = Guid.NewGuid(),
                Id = Guid.NewGuid()
            };
            RaiseTestEvent(@event);
        }
    }

    // Test domain event
    private record TestDomainEvent : DomainEvent;

    // Another test domain event for variety
    private record AnotherTestDomainEvent : DomainEvent;

    #endregion
}