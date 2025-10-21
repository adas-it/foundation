using Adasit.Foundation.Domain;
using Adasit.Foundation.Domain.Validations;
using FluentAssertions;

namespace Adasit.Foundation.DomainTests.Validations;

public class DomainValidationsTests
{
    #region NotNull(object) Tests

    [Fact]
    public void NotNull_Object_WhenTargetIsNull_ShouldReturnNotification()
    {
        // Arrange
        object? target = null;

        // Act
        var notification = target.NotNull();

        var targetName = nameof(target);

        // Assert
        notification.Should().NotBeNull();
        notification!.FieldName.Should().Be(targetName);
        notification.Message.Should().Be(DefaultsErrorsMessages.NotNull.GetMessage(targetName));
        notification.Error.Should().Be(CommonErrorCodes.Validation);
    }

    [Fact]
    public void NotNull_Object_WhenTargetIsNotNull_ShouldReturnNull()
    {
        // Arrange
        var target = new object();

        // Act
        var notification = target.NotNull();

        // Assert
        notification.Should().BeNull();
    }

    [Fact]
    public void NotNull_Object_WhenTargetIsString_ShouldReturnNull()
    {
        // Arrange
        var target = "some value";

        // Act
        var notification = target.NotNull();

        // Assert
        notification.Should().BeNull();
    }

    #endregion

    #region NotNull(Guid) Tests

    [Fact]
    public void NotNull_Guid_WhenTargetIsEmpty_ShouldReturnNotification()
    {
        // Arrange
        var target = Guid.Empty;

        // Act
        var notification = target.NotNull();

        var targetName = nameof(target);

        // Assert
        notification.Should().NotBeNull();
        notification!.FieldName.Should().Be(targetName);
        notification.Message.Should().Be(DefaultsErrorsMessages.NotNull.GetMessage(targetName));
        notification.Error.Should().Be(CommonErrorCodes.Validation);
    }

    [Fact]
    public void NotNull_Guid_WhenTargetIsValid_ShouldReturnNull()
    {
        // Arrange
        var target = Guid.NewGuid();

        // Act
        var notification = target.NotNull();

        // Assert
        notification.Should().BeNull();
    }

    #endregion

    #region NotNullOrEmptyOrWhiteSpace Tests

    [Fact]
    public void NotNullOrEmptyOrWhiteSpace_WhenTargetIsNull_ShouldReturnNotification()
    {
        // Arrange
        string? target = null;

        // Act
        var notification = target.NotNullOrEmptyOrWhiteSpace();
        var targetName = nameof(target);

        // Assert
        notification.Should().NotBeNull();
        notification!.FieldName.Should().Be(targetName);
        notification.Message.Should().Be(DefaultsErrorsMessages.NotNull.GetMessage(targetName));
        notification.Error.Should().Be(CommonErrorCodes.Validation);
    }

    [Fact]
    public void NotNullOrEmptyOrWhiteSpace_WhenTargetIsEmpty_ShouldReturnNotification()
    {
        // Arrange
        var target = string.Empty;

        // Act
        var notification = target.NotNullOrEmptyOrWhiteSpace();
        var targetName = nameof(target);

        // Assert
        notification.Should().NotBeNull();
        notification!.FieldName.Should().Be(targetName);
        notification.Message.Should().Be(DefaultsErrorsMessages.NotNull.GetMessage(targetName));
        notification.Error.Should().Be(CommonErrorCodes.Validation);
    }

    [Fact]
    public void NotNullOrEmptyOrWhiteSpace_WhenTargetIsWhiteSpace_ShouldReturnNotification()
    {
        // Arrange
        var target = "   ";

        // Act
        var notification = target.NotNullOrEmptyOrWhiteSpace();
        var targetName = nameof(target);

        // Assert
        notification.Should().NotBeNull();
        notification!.FieldName.Should().Be(targetName);
        notification.Message.Should().Be(DefaultsErrorsMessages.NotNull.GetMessage(targetName));
        notification.Error.Should().Be(CommonErrorCodes.Validation);
    }

    [Fact]
    public void NotNullOrEmptyOrWhiteSpace_WhenTargetIsValid_ShouldReturnNull()
    {
        // Arrange
        var target = "valid string";

        // Act
        var notification = target.NotNullOrEmptyOrWhiteSpace();

        // Assert
        notification.Should().BeNull();
    }

    #endregion

    #region NotDefaultDateTime(DateTime) Tests

    [Fact]
    public void NotDefaultDateTime_DateTime_WhenTargetIsDefault_ShouldReturnNotification()
    {
        // Arrange
        var target = default(DateTime);

        // Act
        var notification = target.NotDefaultDateTime();

        // Assert
        notification.Should().NotBeNull();
        notification!.FieldName.Should().Be("target");
        notification.Message.Should().Be("The date field target has to have valid data.");
        notification.Error.Should().Be(CommonErrorCodes.Validation);
    }

    [Fact]
    public void NotDefaultDateTime_DateTime_WhenTargetIsValid_ShouldReturnNull()
    {
        // Arrange
        var target = DateTime.Now;

        // Act
        var notification = target.NotDefaultDateTime();

        // Assert
        notification.Should().BeNull();
    }

    [Fact]
    public void NotDefaultDateTime_DateTime_WhenTargetIsMinValue_ShouldReturnNotification()
    {
        // Arrange
        var target = DateTime.MinValue;

        // Act
        var notification = target.NotDefaultDateTime();

        // Assert
        notification.Should().NotBeNull();
        notification!.FieldName.Should().Be("target");
        notification.Message.Should().Be("The date field target has to have valid data.");
    }

    #endregion

    #region NotDefaultDateTime(DateTime?) Tests

    [Fact]
    public void NotDefaultDateTime_NullableDateTime_WhenTargetIsNull_ShouldReturnNull()
    {
        // Arrange
        DateTime? target = null;

        // Act
        var notification = target.NotDefaultDateTime();

        // Assert
        notification.Should().BeNull();
    }

    [Fact]
    public void NotDefaultDateTime_NullableDateTime_WhenTargetIsDefault_ShouldReturnNotification()
    {
        // Arrange
        DateTime? target = default(DateTime);

        // Act
        var notification = target.NotDefaultDateTime();

        // Assert
        notification.Should().NotBeNull();
        notification!.FieldName.Should().Be("target");
        notification.Message.Should().Be("The date field target has to have valid data.");
        notification.Error.Should().Be(CommonErrorCodes.Validation);
    }

    [Fact]
    public void NotDefaultDateTime_NullableDateTime_WhenTargetIsValid_ShouldReturnNull()
    {
        // Arrange
        DateTime? target = DateTime.Now;

        // Act
        var notification = target.NotDefaultDateTime();

        // Assert
        notification.Should().BeNull();
    }

    #endregion

    #region BetweenLength Tests

    [Fact]
    public void BetweenLength_WhenTargetIsNull_ShouldReturnNull()
    {
        // Arrange
        string? target = null;

        // Act
        var notification = target.BetweenLength(3, 10);

        // Assert
        notification.Should().BeNull();
    }

    [Fact]
    public void BetweenLength_WhenTargetIsEmpty_ShouldReturnNull()
    {
        // Arrange
        var target = string.Empty;

        // Act
        var notification = target.BetweenLength(3, 10);

        // Assert
        notification.Should().BeNull();
    }

    [Fact]
    public void BetweenLength_WhenLengthIsBelowMinimum_ShouldReturnNotification()
    {
        // Arrange
        var target = "ab";

        // Act
        var notification = target.BetweenLength(3, 10);

        // Assert
        notification.Should().NotBeNull();
        notification!.FieldName.Should().Be("target");
        notification.Message.Should().Be("The field target has to have length between 3 and 10.");
        notification.Error.Should().Be(CommonErrorCodes.Validation);
    }

    [Fact]
    public void BetweenLength_WhenLengthIsAboveMaximum_ShouldReturnNotification()
    {
        // Arrange
        var target = "this is a very long string";

        // Act
        var notification = target.BetweenLength(3, 10);

        // Assert
        notification.Should().NotBeNull();
        notification!.FieldName.Should().Be("target");
        notification.Message.Should().Be("The field target has to have length between 3 and 10.");
        notification.Error.Should().Be(CommonErrorCodes.Validation);
    }

    [Fact]
    public void BetweenLength_WhenLengthIsAtMinimum_ShouldReturnNull()
    {
        // Arrange
        var target = "abc";

        // Act
        var notification = target.BetweenLength(3, 10);

        // Assert
        notification.Should().BeNull();
    }

    [Fact]
    public void BetweenLength_WhenLengthIsAtMaximum_ShouldReturnNull()
    {
        // Arrange
        var target = "abcdefghij";

        // Act
        var notification = target.BetweenLength(3, 10);

        // Assert
        notification.Should().BeNull();
    }

    [Fact]
    public void BetweenLength_WhenLengthIsWithinRange_ShouldReturnNull()
    {
        // Arrange
        var target = "valid";

        // Act
        var notification = target.BetweenLength(3, 10);

        // Assert
        notification.Should().BeNull();
    }

    #endregion

    #region ValidUrl Tests

    [Fact]
    public void ValidUrl_WhenTargetIsNull_ShouldReturnNull()
    {
        // Arrange
        string? target = null;

        // Act
        var notification = target!.ValidUrl();

        // Assert
        notification.Should().BeNull();
    }

    [Fact]
    public void ValidUrl_WhenTargetIsEmpty_ShouldReturnNull()
    {
        // Arrange
        var target = string.Empty;

        // Act
        var notification = target.ValidUrl();

        // Assert
        notification.Should().BeNull();
    }

    [Fact]
    public void ValidUrl_WhenTargetIsInvalidUrl_ShouldReturnNotification()
    {
        // Arrange
        var target = "not a valid url";

        // Act
        var notification = target.ValidUrl();

        // Assert
        notification.Should().NotBeNull();
        notification!.FieldName.Should().Be("target");
        notification.Message.Should().Be("The field target has to have a valid Url.");
        notification.Error.Should().Be(CommonErrorCodes.Validation);
    }

    [Fact]
    public void ValidUrl_WhenTargetIsRelativeUrl_ShouldReturnNotification()
    {
        // Arrange
        var target = "relative/path";

        // Act
        var notification = target.ValidUrl();

        // Assert
        notification.Should().NotBeNull();
        notification!.FieldName.Should().Be("target");
        notification.Message.Should().Be("The field target has to have a valid Url.");
    }

    [Fact]
    public void ValidUrl_WhenTargetIsValidHttpUrl_ShouldReturnNull()
    {
        // Arrange
        var target = "http://example.com";

        // Act
        var notification = target.ValidUrl();

        // Assert
        notification.Should().BeNull();
    }

    [Fact]
    public void ValidUrl_WhenTargetIsValidHttpsUrl_ShouldReturnNull()
    {
        // Arrange
        var target = "https://example.com";

        // Act
        var notification = target.ValidUrl();

        // Assert
        notification.Should().BeNull();
    }

    [Fact]
    public void ValidUrl_WhenTargetIsValidUrlWithPath_ShouldReturnNull()
    {
        // Arrange
        var target = "https://example.com/path/to/resource";

        // Act
        var notification = target.ValidUrl();

        // Assert
        notification.Should().BeNull();
    }

    [Fact]
    public void ValidUrl_WhenTargetIsValidUrlWithQueryString_ShouldReturnNull()
    {
        // Arrange
        var target = "https://example.com/search?q=test&page=1";

        // Act
        var notification = target.ValidUrl();

        // Assert
        notification.Should().BeNull();
    }

    #endregion
}