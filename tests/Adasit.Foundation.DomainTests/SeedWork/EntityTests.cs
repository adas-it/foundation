using Adasit.Foundation.Domain;
using Adasit.Foundation.Domain.SeedWork;
using Adasit.Foundation.Domain.Validations;
using Adasit.Foundation.Domain.ValuesObjects;
using FluentAssertions;

namespace Adasit.Foundation.DomainTests.SeedWork;

public class EntityTests
{
    #region Constructor Tests

    [Fact]
    public void Constructor_ShouldInitializeIdAutomatically()
    {
        // Act
        var entity = new TestEntity();

        // Assert
        entity.Id.Should().NotBeNull();
        entity.Id.Value.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void Constructor_ShouldInitializeEmptyNotificationsCollections()
    {
        // Act
        var entity = new TestEntity();

        // Assert
        entity.GetNotifications().Should().BeEmpty();
        entity.GetWarnings().Should().BeEmpty();
        entity.GetInformation().Should().BeEmpty();
    }

    [Fact]
    public void Constructor_MultipleInstances_ShouldHaveUniqueIds()
    {
        // Act
        var entity1 = new TestEntity();
        var entity2 = new TestEntity();

        // Assert
        entity1.Id.Should().NotBe(entity2.Id);
    }

    #endregion

    #region Validate Tests

    [Fact]
    public void Validate_WhenIdIsValid_ShouldReturnSuccess()
    {
        // Arrange
        var entity = new TestEntity();

        // Act
        var result = entity.PerformValidation();

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Validate_WhenIdIsValid_ShouldNotAddNotifications()
    {
        // Arrange
        var entity = new TestEntity();

        // Act
        entity.PerformValidation();

        // Assert
        entity.GetNotifications().Should().BeEmpty();
    }

    [Fact]
    public void Validate_WhenValidationFails_ShouldReturnFailure()
    {
        // Arrange
        var entity = new TestEntity();
        entity.AddTestNotification("TestField", "Test error", CommonErrorCodes.Validation);

        // Act
        var result = entity.PerformValidation();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
    }

    [Fact]
    public void Validate_WithWarningsAndNoErrors_ShouldReturnSuccessWithWarnings()
    {
        // Arrange
        var entity = new TestEntity();
        entity.AddTestWarning("TestField", "Test warning", CommonErrorCodes.Validation);

        // Act
        var result = entity.PerformValidation();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Warnings.Should().HaveCount(1);
        result.Errors.Should().BeEmpty();
    }

    #endregion

    #region AddNotification(List<Notification>) Tests

    [Fact]
    public void AddNotification_WithListOfNotifications_ShouldAddAll()
    {
        // Arrange
        var entity = new TestEntity();
        var notifications = new List<Notification>
        {
            new("Field1", "Error 1", CommonErrorCodes.Validation),
            new("Field2", "Error 2", CommonErrorCodes.Validation),
            new("Field3", "Error 3", CommonErrorCodes.Validation)
        };

        // Act
        entity.AddTestNotifications(notifications);

        // Assert
        entity.GetNotifications().Should().HaveCount(3);
        entity.GetNotifications().Should().Contain(notifications);
    }

    [Fact]
    public void AddNotification_WithEmptyList_ShouldNotAddAny()
    {
        // Arrange
        var entity = new TestEntity();
        var notifications = new List<Notification>();

        // Act
        entity.AddTestNotifications(notifications);

        // Assert
        entity.GetNotifications().Should().BeEmpty();
    }

    #endregion

    #region AddNotification(Notification?) Tests

    [Fact]
    public void AddNotification_WithSingleNotification_ShouldAddIt()
    {
        // Arrange
        var entity = new TestEntity();
        var notification = new Notification("Field", "Message", CommonErrorCodes.Validation);

        // Act
        entity.AddTestNotification(notification);

        // Assert
        entity.GetNotifications().Should().HaveCount(1);
        entity.GetNotifications().Should().Contain(notification);
    }

    [Fact]
    public void AddNotification_WithNullNotification_ShouldNotAdd()
    {
        // Arrange
        var entity = new TestEntity();

        // Act
        entity.AddTestNotification(null);

        // Assert
        entity.GetNotifications().Should().BeEmpty();
    }

    [Fact]
    public void AddNotification_WithMultipleCalls_ShouldAddAllNotifications()
    {
        // Arrange
        var entity = new TestEntity();
        var notification1 = new Notification("Field1", "Message1", CommonErrorCodes.Validation);
        var notification2 = new Notification("Field2", "Message2", CommonErrorCodes.Validation);

        // Act
        entity.AddTestNotification(notification1);
        entity.AddTestNotification(notification2);

        // Assert
        entity.GetNotifications().Should().HaveCount(2);
        entity.GetNotifications().Should().Contain(notification1);
        entity.GetNotifications().Should().Contain(notification2);
    }

    #endregion

    #region AddNotification(string, string, DomainErrorCode) Tests

    [Fact]
    public void AddNotification_WithParameters_ShouldCreateAndAddNotification()
    {
        // Arrange
        var entity = new TestEntity();
        var fieldName = "TestField";
        var message = "Test message";
        var errorCode = CommonErrorCodes.Validation;

        // Act
        entity.AddTestNotification(fieldName, message, errorCode);

        // Assert
        var notification = entity.GetNotifications().Should().ContainSingle().Subject;
        notification.FieldName.Should().Be(fieldName);
        notification.Message.Should().Be(message);
        notification.Error.Should().Be(errorCode);
    }

    [Fact]
    public void AddNotification_WithMultipleParameterCalls_ShouldAddAllNotifications()
    {
        // Arrange
        var entity = new TestEntity();

        // Act
        entity.AddTestNotification("Field1", "Message1", CommonErrorCodes.Validation);
        entity.AddTestNotification("Field2", "Message2", CommonErrorCodes.General);

        // Assert
        entity.GetNotifications().Should().HaveCount(2);
    }

    #endregion

    #region AddWarning(Notification?) Tests

    [Fact]
    public void AddWarning_WithSingleNotification_ShouldAddIt()
    {
        // Arrange
        var entity = new TestEntity();
        var warning = new Notification("Field", "Warning message", CommonErrorCodes.Validation);

        // Act
        entity.AddTestWarning(warning);

        // Assert
        entity.GetWarnings().Should().HaveCount(1);
        entity.GetWarnings().Should().Contain(warning);
    }

    [Fact]
    public void AddWarning_WithNullNotification_ShouldNotAdd()
    {
        // Arrange
        var entity = new TestEntity();

        // Act
        entity.AddTestWarning(null);

        // Assert
        entity.GetWarnings().Should().BeEmpty();
    }

    [Fact]
    public void AddWarning_WithMultipleCalls_ShouldAddAllWarnings()
    {
        // Arrange
        var entity = new TestEntity();
        var warning1 = new Notification("Field1", "Warning1", CommonErrorCodes.Validation);
        var warning2 = new Notification("Field2", "Warning2", CommonErrorCodes.Validation);

        // Act
        entity.AddTestWarning(warning1);
        entity.AddTestWarning(warning2);

        // Assert
        entity.GetWarnings().Should().HaveCount(2);
    }

    #endregion

    #region AddWarning(string, string, DomainErrorCode) Tests

    [Fact]
    public void AddWarning_WithParameters_ShouldCreateAndAddWarning()
    {
        // Arrange
        var entity = new TestEntity();
        var fieldName = "TestField";
        var message = "Warning message";
        var errorCode = CommonErrorCodes.Validation;

        // Act
        entity.AddTestWarning(fieldName, message, errorCode);

        // Assert
        var warning = entity.GetWarnings().Should().ContainSingle().Subject;
        warning.FieldName.Should().Be(fieldName);
        warning.Message.Should().Be(message);
        warning.Error.Should().Be(errorCode);
    }

    #endregion

    #region AddInformation(Notification?) Tests

    [Fact]
    public void AddInformation_WithSingleNotification_ShouldAddIt()
    {
        // Arrange
        var entity = new TestEntity();
        var info = new Notification("Field", "Info message", CommonErrorCodes.Validation);

        // Act
        entity.AddTestInformation(info);

        // Assert
        entity.GetInformation().Should().HaveCount(1);
        entity.GetInformation().Should().Contain(info);
    }

    [Fact]
    public void AddInformation_WithNullNotification_ShouldNotAdd()
    {
        // Arrange
        var entity = new TestEntity();

        // Act
        entity.AddTestInformation(null);

        // Assert
        entity.GetInformation().Should().BeEmpty();
    }

    [Fact]
    public void AddInformation_WithMultipleCalls_ShouldAddAllInformation()
    {
        // Arrange
        var entity = new TestEntity();
        var info1 = new Notification("Field1", "Info1", CommonErrorCodes.Validation);
        var info2 = new Notification("Field2", "Info2", CommonErrorCodes.Validation);

        // Act
        entity.AddTestInformation(info1);
        entity.AddTestInformation(info2);

        // Assert
        entity.GetInformation().Should().HaveCount(2);
    }

    #endregion

    #region AddInformation(string, string, DomainErrorCode) Tests

    [Fact]
    public void AddInformation_WithParameters_ShouldCreateAndAddInformation()
    {
        // Arrange
        var entity = new TestEntity();
        var fieldName = "TestField";
        var message = "Info message";
        var errorCode = CommonErrorCodes.Validation;

        // Act
        entity.AddTestInformation(fieldName, message, errorCode);

        // Assert
        var info = entity.GetInformation().Should().ContainSingle().Subject;
        info.FieldName.Should().Be(fieldName);
        info.Message.Should().Be(message);
        info.Error.Should().Be(errorCode);
    }

    #endregion

    #region ValidateAsync Tests

    [Fact]
    public async Task ValidateAsync_WithValidEntity_ShouldReturnSuccessAndEntity()
    {
        // Arrange
        var entity = new TestEntity();
        var validator = new TestValidator(new List<Notification>());

        // Act
        var (result, validatedEntity) = await entity.PerformValidateAsync(validator, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        validatedEntity.Should().NotBeNull();
        validatedEntity.Should().Be(entity);
    }

    [Fact]
    public async Task ValidateAsync_WithValidationErrors_ShouldReturnFailureAndNull()
    {
        // Arrange
        var entity = new TestEntity();
        var validationErrors = new List<Notification>
        {
            new("Field", "Error", CommonErrorCodes.Validation)
        };
        var validator = new TestValidator(validationErrors);

        // Act
        var (result, validatedEntity) = await entity.PerformValidateAsync(validator, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        validatedEntity.Should().BeNull();
        result.Errors.Should().HaveCount(1);
    }

    [Fact]
    public async Task ValidateAsync_ShouldCallValidatorValidateCreationAsync()
    {
        // Arrange
        var entity = new TestEntity();
        var validator = new TestValidator(new List<Notification>());

        // Act
        await entity.PerformValidateAsync(validator, CancellationToken.None);

        // Assert
        validator.WasCalled.Should().BeTrue();
    }

    [Fact]
    public async Task ValidateAsync_WhenCancellationRequested_ShouldThrowOperationCanceledException()
    {
        // Arrange
        var entity = new TestEntity();
        var validator = new TestValidator(new List<Notification>());
        var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act
        var act = async () => await entity.PerformValidateAsync(validator, cts.Token);

        // Assert
        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task ValidateAsync_WithWarnings_ShouldReturnSuccessWithWarnings()
    {
        // Arrange
        var entity = new TestEntity();
        entity.AddTestWarning("Field", "Warning", CommonErrorCodes.Validation);
        var validator = new TestValidator(new List<Notification>());

        // Act
        var (result, validatedEntity) = await entity.PerformValidateAsync(validator, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Warnings.Should().HaveCount(1);
        validatedEntity.Should().NotBeNull();
    }

    [Fact]
    public async Task ValidateAsync_ShouldAddValidatorNotificationsToEntity()
    {
        // Arrange
        var entity = new TestEntity();
        var validationErrors = new List<Notification>
        {
            new("Field1", "Error1", CommonErrorCodes.Validation),
            new("Field2", "Error2", CommonErrorCodes.Validation)
        };
        var validator = new TestValidator(validationErrors);

        // Act
        await entity.PerformValidateAsync(validator, CancellationToken.None);

        // Assert
        entity.GetNotifications().Should().HaveCount(2);
    }

    #endregion

    #region Notifications Property Tests

    [Fact]
    public void Notifications_ShouldReturnReadOnlyCollection()
    {
        // Arrange
        var entity = new TestEntity();
        entity.AddTestNotification("Field", "Error", CommonErrorCodes.Validation);

        // Act
        var notifications = entity.GetNotifications();

        // Assert
        notifications.Should().BeAssignableTo<IReadOnlyCollection<Notification>>();
    }

    [Fact]
    public void Notifications_ShouldReturnNewCollectionOnEachAccess()
    {
        // Arrange
        var entity = new TestEntity();

        // Act
        var notifications1 = entity.GetNotifications();
        entity.AddTestNotification("Field", "Error", CommonErrorCodes.Validation);
        var notifications2 = entity.GetNotifications();

        // Assert
        notifications1.Should().NotBeSameAs(notifications2);
        notifications1.Should().HaveCount(0);
        notifications2.Should().HaveCount(1);
    }

    #endregion

    #region Warnings Property Tests

    [Fact]
    public void Warnings_ShouldReturnReadOnlyCollection()
    {
        // Arrange
        var entity = new TestEntity();
        entity.AddTestWarning("Field", "Warning", CommonErrorCodes.Validation);

        // Act
        var warnings = entity.GetWarnings();

        // Assert
        warnings.Should().BeAssignableTo<IReadOnlyCollection<Notification>>();
    }

    #endregion

    #region Information Property Tests

    [Fact]
    public void Information_ShouldReturnReadOnlyCollection()
    {
        // Arrange
        var entity = new TestEntity();
        entity.AddTestInformation("Field", "Info", CommonErrorCodes.Validation);

        // Act
        var information = entity.GetInformation();

        // Assert
        information.Should().BeAssignableTo<IReadOnlyCollection<Notification>>();
    }

    #endregion

    #region Integration Tests

    [Fact]
    public void Entity_MixedNotificationsWarningsAndInfo_ShouldKeepThemSeparate()
    {
        // Arrange
        var entity = new TestEntity();

        // Act
        entity.AddTestNotification("Field1", "Error", CommonErrorCodes.Validation);
        entity.AddTestWarning("Field2", "Warning", CommonErrorCodes.Validation);
        entity.AddTestInformation("Field3", "Info", CommonErrorCodes.Validation);

        // Assert
        entity.GetNotifications().Should().HaveCount(1);
        entity.GetWarnings().Should().HaveCount(1);
        entity.GetInformation().Should().HaveCount(1);
    }

    [Fact]
    public async Task Entity_CompleteValidationWorkflow_ShouldWorkCorrectly()
    {
        // Arrange
        var entity = new TestEntity();
        var validator = new TestValidator(new List<Notification>());

        // Add some warnings and info before validation
        entity.AddTestWarning("Field1", "Warning", CommonErrorCodes.Validation);
        entity.AddTestInformation("Field2", "Info", CommonErrorCodes.Validation);

        // Act
        var (result, validatedEntity) = await entity.PerformValidateAsync(validator, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Warnings.Should().HaveCount(1);
        validatedEntity.Should().NotBeNull();
        entity.GetNotifications().Should().BeEmpty();
        entity.GetWarnings().Should().HaveCount(1);
        entity.GetInformation().Should().HaveCount(1);
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
        // Expose protected methods for testing
        public DomainResult PerformValidation() => Validate();

        public void AddTestNotifications(List<Notification> notifications) => AddNotification(notifications);

        public void AddTestNotification(Notification? notification) => AddNotification(notification);

        public void AddTestNotification(string fieldName, string message, DomainErrorCode domainError)
            => AddNotification(fieldName, message, domainError);

        public void AddTestWarning(Notification? notification) => AddWarning(notification);

        public void AddTestWarning(string fieldName, string message, DomainErrorCode domainError)
            => AddWarning(fieldName, message, domainError);

        public void AddTestInformation(Notification? notification) => AddInformation(notification);

        public void AddTestInformation(string fieldName, string message, DomainErrorCode domainError)
            => AddInformation(fieldName, message, domainError);

        public async Task<(DomainResult, TestEntity?)> PerformValidateAsync(
            IDefaultValidator<TestEntity, TestEntityId> validator,
            CancellationToken cancellationToken)
            => await ValidateAsync(validator, this, cancellationToken);

        // Expose protected properties for testing
        public IReadOnlyCollection<Notification> GetNotifications() => Notifications;
        public IReadOnlyCollection<Notification> GetWarnings() => Warnings;
        public IReadOnlyCollection<Notification> GetInformation() => Information;
    }

    // Test validator implementation
    private class TestValidator : IDefaultValidator<TestEntity, TestEntityId>
    {
        private readonly List<Notification> _notifications;
        public bool WasCalled { get; private set; }

        public TestValidator(List<Notification> notifications)
        {
            _notifications = notifications;
        }

        public Task<List<Notification>> ValidateCreationAsync(TestEntity entity, CancellationToken cancellationToken)
        {
            WasCalled = true;
            return Task.FromResult(_notifications);
        }
    }

    #endregion
}