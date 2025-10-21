using Adasit.Foundation.Domain.Validations;
using FluentAssertions;

namespace Adasit.Foundation.DomainTests.Validations;

public class DefaultsErrorsMessagesTests
{
    #region GetMessage Tests

    [Fact]
    public void GetMessage_WhenCalledWithSingleParameter_ShouldFormatCorrectly()
    {
        // Arrange
        var template = "The field {0} is required.";
        var fieldName = "Name";

        // Act
        var result = template.GetMessage(fieldName);

        // Assert
        result.Should().Be("The field Name is required.");
    }

    [Fact]
    public void GetMessage_WhenCalledWithMultipleParameters_ShouldFormatCorrectly()
    {
        // Arrange
        var template = "The field {0} has to have length between {1} and {2}.";
        var fieldName = "Description";
        var minLength = 10;
        var maxLength = 500;

        // Act
        var result = template.GetMessage(fieldName, minLength, maxLength);

        // Assert
        result.Should().Be("The field Description has to have length between 10 and 500.");
    }

    [Fact]
    public void GetMessage_WhenCalledWithNoParameters_ShouldReturnTemplateAsIs()
    {
        // Arrange
        var template = "The value provided is not a valid GUID.";

        // Act
        var result = template.GetMessage();

        // Assert
        result.Should().Be("The value provided is not a valid GUID.");
    }

    [Fact]
    public void GetMessage_WhenCalledWithNullParameters_ShouldHandleNulls()
    {
        // Arrange
        var template = "{0} has to be greater than {1}.";

        // Act
        var result = template.GetMessage(null, null);

        // Assert
        result.Should().Be(" has to be greater than .");
    }

    [Fact]
    public void GetMessage_WhenCalledWithExtraParameters_ShouldIgnoreExtraParameters()
    {
        // Arrange
        var template = "The field {0} is required.";

        // Act
        var result = template.GetMessage("Name", "extra");

        // Assert
        result.Should().Be("The field Name is required.");
    }

    [Fact]
    public void GetMessage_WhenCalledWithInsufficientParameters_ShouldThrowFormatException()
    {
        // Arrange
        var template = "The field {0} has to have length between {1} and {2}.";

        // Act & Assert
        Assert.Throws<FormatException>(() => template.GetMessage("Name"));
    }

    #endregion

    #region Message Template Integration Tests

    [Fact]
    public void NotNull_GetMessage_ShouldFormatCorrectly()
    {
        // Act
        var result = DefaultsErrorsMessages.NotNull.GetMessage("Email");

        // Assert
        result.Should().Be("The field Email is required.");
    }

    [Fact]
    public void NotDefaultDateTime_GetMessage_ShouldFormatCorrectly()
    {
        // Act
        var result = DefaultsErrorsMessages.NotDefaultDateTime.GetMessage("CreatedAt");

        // Assert
        result.Should().Be("The date field CreatedAt has to have valid data.");
    }

    [Fact]
    public void BetweenLength_GetMessage_ShouldFormatCorrectly()
    {
        // Act
        var result = DefaultsErrorsMessages.BetweenLength.GetMessage("Title", 5, 100);

        // Assert
        result.Should().Be("The field Title has to have length between 5 and 100.");
    }

    [Fact]
    public void Date0CannotBeBeforeDate1_GetMessage_ShouldFormatCorrectly()
    {
        // Act
        var result = DefaultsErrorsMessages.Date0CannotBeBeforeDate1.GetMessage("EndDate", "StartDate");

        // Assert
        result.Should().Be("EndDate has to be greater than StartDate.");
    }

    [Fact]
    public void InvalidUrl_GetMessage_ShouldFormatCorrectly()
    {
        // Act
        var result = DefaultsErrorsMessages.InvalidUrl.GetMessage("Website");

        // Assert
        result.Should().Be("The field Website has to have a valid Url.");
    }

    [Fact]
    public void InvalidGuid_GetMessage_ShouldFormatCorrectly()
    {
        // Act
        var result = DefaultsErrorsMessages.InvalidGuid.GetMessage();

        // Assert
        result.Should().Be("The value provided is not a valid GUID.");
    }

    #endregion
}
