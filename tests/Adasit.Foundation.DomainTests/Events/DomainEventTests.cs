using Adasit.Foundation.Domain.Events;
using FluentAssertions;

namespace Adasit.Foundation.DomainTests.Events;

public class DomainEventTests
{
    #region Constructor and Default Values Tests

    [Fact]
    public void Constructor_ShouldGenerateUniqueEventId()
    {
        // Act
        var event1 = new TestDomainEvent();
        var event2 = new TestDomainEvent();

        // Assert
        event1.EventId.Should().NotBe(Guid.Empty);
        event2.EventId.Should().NotBe(Guid.Empty);
        event1.EventId.Should().NotBe(event2.EventId);
    }

    [Fact]
    public void Constructor_ShouldSetEventDateToUtcNow()
    {
        // Arrange
        var beforeCreation = DateTime.UtcNow;

        System.Threading.Thread.Sleep(5);
        // Act
        var domainEvent = new TestDomainEvent();

        // Assert

        System.Threading.Thread.Sleep(5);
        var afterCreation = DateTime.UtcNow;
        domainEvent.EventDate.Should().BeOnOrAfter(beforeCreation);
        domainEvent.EventDate.Should().BeOnOrBefore(afterCreation);
        domainEvent.EventDate.Kind.Should().Be(DateTimeKind.Utc);
    }

    [Fact]
    public void Constructor_ShouldInitializeUserIdAsEmpty()
    {
        // Act
        var domainEvent = new TestDomainEvent();

        // Assert
        domainEvent.UserId.Should().Be(Guid.Empty);
    }

    [Fact]
    public void Constructor_ShouldInitializeIdAsEmpty()
    {
        // Act
        var domainEvent = new TestDomainEvent();

        // Assert
        domainEvent.Id.Should().Be(Guid.Empty);
    }

    #endregion

    #region Property Initialization Tests

    [Fact]
    public void Init_ShouldAllowSettingEventId()
    {
        // Arrange
        var customEventId = Guid.NewGuid();

        // Act
        var domainEvent = new TestDomainEvent { EventId = customEventId };

        // Assert
        domainEvent.EventId.Should().Be(customEventId);
    }

    [Fact]
    public void Init_ShouldAllowSettingEventDate()
    {
        // Arrange
        var customDate = new DateTime(2024, 1, 15, 10, 30, 0, DateTimeKind.Utc);

        // Act
        var domainEvent = new TestDomainEvent { EventDate = customDate };

        // Assert
        domainEvent.EventDate.Should().Be(customDate);
    }

    [Fact]
    public void Init_ShouldAllowSettingUserId()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        var domainEvent = new TestDomainEvent { UserId = userId };

        // Assert
        domainEvent.UserId.Should().Be(userId);
    }

    [Fact]
    public void Init_ShouldAllowSettingId()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var domainEvent = new TestDomainEvent { Id = id };

        // Assert
        domainEvent.Id.Should().Be(id);
    }

    [Fact]
    public void Init_ShouldAllowSettingAllProperties()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var eventDate = new DateTime(2024, 1, 15, 10, 30, 0, DateTimeKind.Utc);
        var userId = Guid.NewGuid();
        var id = Guid.NewGuid();

        // Act
        var domainEvent = new TestDomainEvent
        {
            EventId = eventId,
            EventDate = eventDate,
            UserId = userId,
            Id = id
        };

        // Assert
        domainEvent.EventId.Should().Be(eventId);
        domainEvent.EventDate.Should().Be(eventDate);
        domainEvent.UserId.Should().Be(userId);
        domainEvent.Id.Should().Be(id);
    }

    #endregion

    #region Record Equality Tests

    [Fact]
    public void Equality_WhenAllPropertiesAreEqual_ShouldBeEqual()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var eventDate = DateTime.UtcNow;
        var userId = Guid.NewGuid();
        var id = Guid.NewGuid();

        var event1 = new TestDomainEvent
        {
            EventId = eventId,
            EventDate = eventDate,
            UserId = userId,
            Id = id
        };

        var event2 = new TestDomainEvent
        {
            EventId = eventId,
            EventDate = eventDate,
            UserId = userId,
            Id = id
        };

        // Act & Assert
        event1.Should().Be(event2);
        event1.Equals(event2).Should().BeTrue();
        (event1 == event2).Should().BeTrue();
        (event1 != event2).Should().BeFalse();
    }

    [Fact]
    public void Equality_WhenEventIdDiffers_ShouldNotBeEqual()
    {
        // Arrange
        var eventDate = DateTime.UtcNow;
        var userId = Guid.NewGuid();
        var id = Guid.NewGuid();

        var event1 = new TestDomainEvent
        {
            EventId = Guid.NewGuid(),
            EventDate = eventDate,
            UserId = userId,
            Id = id
        };

        var event2 = new TestDomainEvent
        {
            EventId = Guid.NewGuid(),
            EventDate = eventDate,
            UserId = userId,
            Id = id
        };

        // Act & Assert
        event1.Should().NotBe(event2);
        event1.Equals(event2).Should().BeFalse();
        (event1 == event2).Should().BeFalse();
        (event1 != event2).Should().BeTrue();
    }

    [Fact]
    public void Equality_WhenUserIdDiffers_ShouldNotBeEqual()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var eventDate = DateTime.UtcNow;
        var id = Guid.NewGuid();

        var event1 = new TestDomainEvent
        {
            EventId = eventId,
            EventDate = eventDate,
            UserId = Guid.NewGuid(),
            Id = id
        };

        var event2 = new TestDomainEvent
        {
            EventId = eventId,
            EventDate = eventDate,
            UserId = Guid.NewGuid(),
            Id = id
        };

        // Act & Assert
        event1.Should().NotBe(event2);
    }

    [Fact]
    public void GetHashCode_WhenEqual_ShouldReturnSameHashCode()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var eventDate = DateTime.UtcNow;
        var userId = Guid.NewGuid();
        var id = Guid.NewGuid();

        var event1 = new TestDomainEvent
        {
            EventId = eventId,
            EventDate = eventDate,
            UserId = userId,
            Id = id
        };

        var event2 = new TestDomainEvent
        {
            EventId = eventId,
            EventDate = eventDate,
            UserId = userId,
            Id = id
        };

        // Act & Assert
        event1.GetHashCode().Should().Be(event2.GetHashCode());
    }

    #endregion

    #region With-Expression Tests

    [Fact]
    public void With_ShouldCreateCopyWithModifiedEventId()
    {
        // Arrange
        var originalEvent = new TestDomainEvent
        {
            EventId = Guid.NewGuid(),
            EventDate = DateTime.UtcNow,
            UserId = Guid.NewGuid(),
            Id = Guid.NewGuid()
        };

        var newEventId = Guid.NewGuid();

        // Act
        var modifiedEvent = originalEvent with { EventId = newEventId };

        // Assert
        modifiedEvent.EventId.Should().Be(newEventId);
        modifiedEvent.EventDate.Should().Be(originalEvent.EventDate);
        modifiedEvent.UserId.Should().Be(originalEvent.UserId);
        modifiedEvent.Id.Should().Be(originalEvent.Id);
    }

    [Fact]
    public void With_ShouldCreateCopyWithModifiedUserId()
    {
        // Arrange
        var originalEvent = new TestDomainEvent
        {
            UserId = Guid.NewGuid(),
            Id = Guid.NewGuid()
        };

        var newUserId = Guid.NewGuid();

        // Act
        var modifiedEvent = originalEvent with { UserId = newUserId };

        // Assert
        modifiedEvent.UserId.Should().Be(newUserId);
        modifiedEvent.EventId.Should().Be(originalEvent.EventId);
        modifiedEvent.Id.Should().Be(originalEvent.Id);
    }

    [Fact]
    public void With_ShouldCreateCopyWithMultipleModifications()
    {
        // Arrange
        var originalEvent = new TestDomainEvent();
        var newUserId = Guid.NewGuid();
        var newId = Guid.NewGuid();

        // Act
        var modifiedEvent = originalEvent with
        {
            UserId = newUserId,
            Id = newId
        };

        // Assert
        modifiedEvent.UserId.Should().Be(newUserId);
        modifiedEvent.Id.Should().Be(newId);
        modifiedEvent.EventId.Should().Be(originalEvent.EventId);
        modifiedEvent.EventDate.Should().Be(originalEvent.EventDate);
    }

    #endregion

    #region ToString Tests

    [Fact]
    public void ToString_ShouldReturnStringRepresentation()
    {
        // Arrange
        var domainEvent = new TestDomainEvent
        {
            EventId = Guid.Parse("12345678-1234-1234-1234-123456789012"),
            UserId = Guid.Parse("87654321-4321-4321-4321-210987654321"),
            Id = Guid.Parse("11111111-2222-3333-4444-555555555555")
        };

        // Act
        var result = domainEvent.ToString();

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain("EventId");
        result.Should().Contain("UserId");
        result.Should().Contain("Id");
    }

    #endregion

    // Test implementation of DomainEvent for testing purposes
    private record TestDomainEvent : DomainEvent;
}