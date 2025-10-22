using Adasit.Foundation.Domain;
using Adasit.Foundation.Domain.ValuesObjects;
using FluentAssertions;

namespace Adasit.Foundation.DomainTests.ValuesObjects;

public class NotificationTests
{
    #region Constructor Tests - With FieldName

    [Fact]
    public void Constructor_WithFieldName_ShouldCreateNotification()
    {
        // Arrange
        var fieldName = "Email";
        var message = "Email is required";
        var errorCode = DomainErrorCode.New(1000);

        // Act
        var notification = new Notification(fieldName, message, errorCode);

        // Assert
        notification.Should().NotBeNull();
        notification.FieldName.Should().Be(fieldName);
        notification.Message.Should().Be(message);
        notification.Error.Should().Be(errorCode);
    }

    [Fact]
    public void Constructor_WithFieldName_WhenFieldNameIsEmpty_ShouldCreateNotification()
    {
        // Arrange
        var fieldName = string.Empty;
        var message = "General error";
        var errorCode = DomainErrorCode.New(1001);

        // Act
        var notification = new Notification(fieldName, message, errorCode);

        // Assert
        notification.FieldName.Should().Be(string.Empty);
        notification.Message.Should().Be(message);
    }

    [Fact]
    public void Constructor_WithFieldName_WhenAllParametersProvided_ShouldSetAllProperties()
    {
        // Arrange
        var fieldName = "Name";
        var message = "Name must be between 3 and 70 characters";
        var errorCode = CommonErrorCodes.Validation;

        // Act
        var notification = new Notification(fieldName, message, errorCode);

        // Assert
        notification.FieldName.Should().Be(fieldName);
        notification.Message.Should().Be(message);
        notification.Error.Should().Be(errorCode);
    }

    #endregion

    #region Constructor Tests - Without FieldName

    [Fact]
    public void Constructor_WithoutFieldName_ShouldCreateNotificationWithEmptyFieldName()
    {
        // Arrange
        var message = "General validation error";
        var errorCode = DomainErrorCode.New(2000);

        // Act
        var notification = new Notification(message, errorCode);

        // Assert
        notification.Should().NotBeNull();
        notification.FieldName.Should().Be(string.Empty);
        notification.Message.Should().Be(message);
        notification.Error.Should().Be(errorCode);
    }

    [Fact]
    public void Constructor_WithoutFieldName_WhenMessageIsEmpty_ShouldCreateNotification()
    {
        // Arrange
        var message = string.Empty;
        var errorCode = DomainErrorCode.New(3000);

        // Act
        var notification = new Notification(message, errorCode);

        // Assert
        notification.Message.Should().Be(string.Empty);
        notification.FieldName.Should().Be(string.Empty);
    }

    #endregion

    #region ToString Tests

    [Fact]
    public void ToString_WithFieldName_ShouldReturnFormattedString()
    {
        // Arrange
        var notification = new Notification("Age", "Age must be positive", DomainErrorCode.New(1002));

        // Act
        var result = notification.ToString();

        // Assert
        result.Should().Be("Field: Age - Error: 1002: Message - Age must be positive");
    }

    [Fact]
    public void ToString_WithoutFieldName_ShouldReturnFormattedStringWithEmptyField()
    {
        // Arrange
        var notification = new Notification("System error occurred", DomainErrorCode.New(5000));

        // Act
        var result = notification.ToString();

        // Assert
        result.Should().Be("Field:  - Error: 5000: Message - System error occurred");
    }

    [Fact]
    public void ToString_WithCommonErrorCode_ShouldIncludeErrorCodeValue()
    {
        // Arrange
        var notification = new Notification("Field", "Error message", CommonErrorCodes.Validation);

        // Act
        var result = notification.ToString();

        // Assert
        result.Should().Contain($"Error: {(int)CommonErrorCodes.Validation}");
    }

    #endregion

    #region Record Equality Tests

    [Fact]
    public void Equality_WhenAllPropertiesAreEqual_ShouldBeEqual()
    {
        // Arrange
        var errorCode = DomainErrorCode.New(100);
        var notification1 = new Notification("Field1", "Message1", errorCode);
        var notification2 = new Notification("Field1", "Message1", errorCode);

        // Act & Assert
        notification1.Should().Be(notification2);
        (notification1 == notification2).Should().BeTrue();
        notification1.Equals(notification2).Should().BeTrue();
    }

    [Fact]
    public void Equality_WhenFieldNameDiffers_ShouldNotBeEqual()
    {
        // Arrange
        var errorCode = DomainErrorCode.New(100);
        var notification1 = new Notification("Field1", "Message", errorCode);
        var notification2 = new Notification("Field2", "Message", errorCode);

        // Act & Assert
        notification1.Should().NotBe(notification2);
        (notification1 != notification2).Should().BeTrue();
    }

    [Fact]
    public void Equality_WhenMessageDiffers_ShouldNotBeEqual()
    {
        // Arrange
        var errorCode = DomainErrorCode.New(100);
        var notification1 = new Notification("Field", "Message1", errorCode);
        var notification2 = new Notification("Field", "Message2", errorCode);

        // Act & Assert
        notification1.Should().NotBe(notification2);
    }

    [Fact]
    public void Equality_WhenErrorCodeDiffers_ShouldNotBeEqual()
    {
        // Arrange
        var notification1 = new Notification("Field", "Message", DomainErrorCode.New(100));
        var notification2 = new Notification("Field", "Message", DomainErrorCode.New(200));

        // Act & Assert
        notification1.Should().NotBe(notification2);
    }

    [Fact]
    public void GetHashCode_WhenEqual_ShouldReturnSameHashCode()
    {
        // Arrange
        var errorCode = DomainErrorCode.New(100);
        var notification1 = new Notification("Field", "Message", errorCode);
        var notification2 = new Notification("Field", "Message", errorCode);

        // Act & Assert
        notification1.GetHashCode().Should().Be(notification2.GetHashCode());
    }

    #endregion

    #region With Expression Tests

    [Fact]
    public void With_ShouldCreateNewInstanceWithModifiedFieldName()
    {
        // Arrange
        var original = new Notification("OldField", "Message", DomainErrorCode.New(100));

        // Act
        var modified = original with { FieldName = "NewField" };

        // Assert
        modified.FieldName.Should().Be("NewField");
        modified.Message.Should().Be(original.Message);
        modified.Error.Should().Be(original.Error);
        original.FieldName.Should().Be("OldField");
    }

    [Fact]
    public void With_ShouldCreateNewInstanceWithModifiedMessage()
    {
        // Arrange
        var original = new Notification("Field", "OldMessage", DomainErrorCode.New(100));

        // Act
        var modified = original with { Message = "NewMessage" };

        // Assert
        modified.Message.Should().Be("NewMessage");
        modified.FieldName.Should().Be(original.FieldName);
        original.Message.Should().Be("OldMessage");
    }

    #endregion

    #region Integration Tests

    [Fact]
    public void Notification_WhenUsedWithCommonErrorCodes_ShouldWorkCorrectly()
    {
        // Arrange & Act
        var validationNotification = new Notification("Email", "Invalid email", CommonErrorCodes.Validation);
        var generalNotification = new Notification("Error occurred", CommonErrorCodes.General);
        var internalNotification = new Notification("System", "Internal error", CommonErrorCodes.Internal);

        // Assert
        validationNotification.Error.Should().Be(CommonErrorCodes.Validation);
        generalNotification.Error.Should().Be(CommonErrorCodes.General);
        internalNotification.Error.Should().Be(CommonErrorCodes.Internal);
    }

    #endregion
}