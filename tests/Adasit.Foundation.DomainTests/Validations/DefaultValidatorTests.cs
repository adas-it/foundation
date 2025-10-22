using Adasit.Foundation.Domain;
using Adasit.Foundation.Domain.SeedWork;
using Adasit.Foundation.Domain.Validations;
using Adasit.Foundation.Domain.ValuesObjects;
using FluentAssertions;

namespace Adasit.Foundation.DomainTests.Validations;

public class DefaultValidatorTests
{
    #region ValidateCreationAsync Tests

    [Fact]
    public async Task ValidateCreationAsync_WithValidEntity_ShouldReturnEmptyNotifications()
    {
        // Arrange
        var validator = new DefaultValidator<TestEntity, TestEntityId>();
        var entity = new TestEntity();

        // Act
        var notifications = await validator.ValidateCreationAsync(entity, CancellationToken.None);

        // Assert
        notifications.Should().NotBeNull();
        notifications.Should().BeEmpty();
    }

    [Fact]
    public async Task ValidateCreationAsync_WithNullEntity_ShouldThrowArgumentNullException()
    {
        // Arrange
        var validator = new DefaultValidator<TestEntity, TestEntityId>();
        TestEntity entity = null!;

        // Act
        var act = async () => await validator.ValidateCreationAsync(entity, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>()
            .WithParameterName("entity");
    }

    [Fact]
    public async Task ValidateCreationAsync_WhenCancellationRequested_ShouldThrowOperationCanceledException()
    {
        // Arrange
        var validator = new DefaultValidator<TestEntity, TestEntityId>();
        var entity = new TestEntity();
        var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act
        var act = async () => await validator.ValidateCreationAsync(entity, cts.Token);

        // Assert
        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task ValidateCreationAsync_MultipleCallsWithSameEntity_ShouldReturnEmptyNotificationsEachTime()
    {
        // Arrange
        var validator = new DefaultValidator<TestEntity, TestEntityId>();
        var entity = new TestEntity();

        // Act
        var notifications1 = await validator.ValidateCreationAsync(entity, CancellationToken.None);
        var notifications2 = await validator.ValidateCreationAsync(entity, CancellationToken.None);

        // Assert
        notifications1.Should().BeEmpty();
        notifications2.Should().BeEmpty();
        notifications1.Should().NotBeSameAs(notifications2);
    }

    [Fact]
    public async Task ValidateCreationAsync_MultipleCallsWithDifferentEntities_ShouldReturnEmptyNotificationsEachTime()
    {
        // Arrange
        var validator = new DefaultValidator<TestEntity, TestEntityId>();
        var entity1 = new TestEntity();
        var entity2 = new TestEntity();

        // Act
        var notifications1 = await validator.ValidateCreationAsync(entity1, CancellationToken.None);
        var notifications2 = await validator.ValidateCreationAsync(entity2, CancellationToken.None);

        // Assert
        notifications1.Should().BeEmpty();
        notifications2.Should().BeEmpty();
    }

    #endregion

    #region Custom Validator Tests

    [Fact]
    public async Task ValidateCreationAsync_WithCustomValidator_ShouldReturnCustomNotifications()
    {
        // Arrange
        var validator = new CustomValidator();
        var entity = new TestEntity();

        // Act
        var notifications = await validator.ValidateCreationAsync(entity, CancellationToken.None);

        // Assert
        notifications.Should().NotBeEmpty();
        notifications.Should().HaveCount(1);
        notifications[0].FieldName.Should().Be("CustomField");
        notifications[0].Message.Should().Be("Custom validation error");
        notifications[0].Error.Should().Be(CommonErrorCodes.Validation);
    }

    [Fact]
    public async Task ValidateCreationAsync_WithCustomValidatorAddingMultipleNotifications_ShouldReturnAllNotifications()
    {
        // Arrange
        var validator = new MultipleNotificationsValidator();
        var entity = new TestEntity();

        // Act
        var notifications = await validator.ValidateCreationAsync(entity, CancellationToken.None);

        // Assert
        notifications.Should().HaveCount(3);
        notifications[0].FieldName.Should().Be("Field1");
        notifications[1].FieldName.Should().Be("Field2");
        notifications[2].FieldName.Should().Be("Field3");
    }

    [Fact]
    public async Task ValidateCreationAsync_WithCustomValidatorReturningNull_ShouldNotThrow()
    {
        // Arrange
        var validator = new NullNotificationValidator();
        var entity = new TestEntity();

        // Act
        var notifications = await validator.ValidateCreationAsync(entity, CancellationToken.None);

        // Assert
        notifications.Should().NotBeNull();
        notifications.Should().BeEmpty();
    }

    #endregion

    #region AddNotification Helper Method Tests

    [Fact]
    public void AddNotification_WithValidNotification_ShouldAddToList()
    {
        // Arrange
        var notification = new Notification("Field", "Message", CommonErrorCodes.Validation);
        var list = new List<Notification>();

        // Act
        TestableDefaultValidator.CallAddNotification(notification, list);

        // Assert
        list.Should().ContainSingle();
        list[0].Should().Be(notification);
    }

    [Fact]
    public void AddNotification_WithNullNotification_ShouldNotAddToList()
    {
        // Arrange
        Notification? notification = null;
        var list = new List<Notification>();

        // Act
        TestableDefaultValidator.CallAddNotification(notification, list);

        // Assert
        list.Should().BeEmpty();
    }

    [Fact]
    public void AddNotification_WithMultipleNotifications_ShouldAddAll()
    {
        // Arrange
        var notification1 = new Notification("Field1", "Message1", CommonErrorCodes.Validation);
        var notification2 = new Notification("Field2", "Message2", CommonErrorCodes.Validation);
        var list = new List<Notification>();

        // Act
        TestableDefaultValidator.CallAddNotification(notification1, list);
        TestableDefaultValidator.CallAddNotification(notification2, list);

        // Assert
        list.Should().HaveCount(2);
        list[0].Should().Be(notification1);
        list[1].Should().Be(notification2);
    }

    [Fact]
    public void AddNotification_WithMixedNullAndValidNotifications_ShouldOnlyAddValid()
    {
        // Arrange
        var notification1 = new Notification("Field1", "Message1", CommonErrorCodes.Validation);
        Notification? notification2 = null;
        var notification3 = new Notification("Field3", "Message3", CommonErrorCodes.Validation);
        var list = new List<Notification>();

        // Act
        TestableDefaultValidator.CallAddNotification(notification1, list);
        TestableDefaultValidator.CallAddNotification(notification2, list);
        TestableDefaultValidator.CallAddNotification(notification3, list);

        // Assert
        list.Should().HaveCount(2);
        list[0].Should().Be(notification1);
        list[1].Should().Be(notification3);
    }

    #endregion

    #region DefaultValidationsAsync Protected Method Tests

    [Fact]
    public async Task DefaultValidationsAsync_WithValidEntity_ShouldNotThrow()
    {
        // Arrange
        var validator = new TestableDefaultValidator();
        var entity = new TestEntity();
        var notifications = new List<Notification>();

        // Act
        var act = async () => await validator.CallDefaultValidationsAsync(entity, notifications, CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task DefaultValidationsAsync_WithNullEntity_ShouldThrowArgumentNullException()
    {
        // Arrange
        var validator = new TestableDefaultValidator();
        TestEntity entity = null!;
        var notifications = new List<Notification>();

        // Act
        var act = async () => await validator.CallDefaultValidationsAsync(entity, notifications, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>()
            .WithParameterName("entity");
    }

    [Fact]
    public async Task DefaultValidationsAsync_WithNullNotificationsList_ShouldThrowArgumentNullException()
    {
        // Arrange
        var validator = new TestableDefaultValidator();
        var entity = new TestEntity();
        List<Notification> notifications = null!;

        // Act
        var act = async () => await validator.CallDefaultValidationsAsync(entity, notifications, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>()
            .WithParameterName("notifications");
    }

    [Fact]
    public async Task DefaultValidationsAsync_WhenCancellationRequested_ShouldThrowOperationCanceledException()
    {
        // Arrange
        var validator = new TestableDefaultValidator();
        var entity = new TestEntity();
        var notifications = new List<Notification>();
        var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act
        var act = async () => await validator.CallDefaultValidationsAsync(entity, notifications, cts.Token);

        // Assert
        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task DefaultValidationsAsync_ShouldCompleteSuccessfully()
    {
        // Arrange
        var validator = new TestableDefaultValidator();
        var entity = new TestEntity();
        var notifications = new List<Notification>();

        // Act
        await validator.CallDefaultValidationsAsync(entity, notifications, CancellationToken.None);

        // Assert
        notifications.Should().BeEmpty();
    }

    #endregion

    #region Test Helpers

    // Test entity ID implementation
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

    // Test entity implementation
    private class TestEntity : Entity<TestEntityId>
    {
    }

    // Testable validator to expose protected methods
    private class TestableDefaultValidator : DefaultValidator<TestEntity, TestEntityId>
    {
        public static void CallAddNotification(Notification? notification, List<Notification> list)
        {
            AddNotification(notification, list);
        }

        public Task CallDefaultValidationsAsync(
            TestEntity entity,
            List<Notification> notifications,
            CancellationToken cancellationToken)
        {
            return DefaultValidationsAsync(entity, notifications, cancellationToken);
        }
    }

    // Custom validator that adds a notification
    private class CustomValidator : DefaultValidator<TestEntity, TestEntityId>
    {
        protected override Task DefaultValidationsAsync(
            TestEntity entity,
            List<Notification> notifications,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ArgumentNullException.ThrowIfNull(entity);
            ArgumentNullException.ThrowIfNull(notifications);

            var notification = new Notification("CustomField", "Custom validation error", CommonErrorCodes.Validation);
            AddNotification(notification, notifications);

            return Task.CompletedTask;
        }
    }

    // Custom validator that adds multiple notifications
    private class MultipleNotificationsValidator : DefaultValidator<TestEntity, TestEntityId>
    {
        protected override Task DefaultValidationsAsync(
            TestEntity entity,
            List<Notification> notifications,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ArgumentNullException.ThrowIfNull(entity);
            ArgumentNullException.ThrowIfNull(notifications);

            AddNotification(new Notification("Field1", "Error1", CommonErrorCodes.Validation), notifications);
            AddNotification(new Notification("Field2", "Error2", CommonErrorCodes.Validation), notifications);
            AddNotification(new Notification("Field3", "Error3", CommonErrorCodes.Validation), notifications);

            return Task.CompletedTask;
        }
    }

    // Custom validator that tries to add null notification
    private class NullNotificationValidator : DefaultValidator<TestEntity, TestEntityId>
    {
        protected override Task DefaultValidationsAsync(
            TestEntity entity,
            List<Notification> notifications,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ArgumentNullException.ThrowIfNull(entity);
            ArgumentNullException.ThrowIfNull(notifications);

            AddNotification(null, notifications);

            return Task.CompletedTask;
        }
    }

    #endregion
}