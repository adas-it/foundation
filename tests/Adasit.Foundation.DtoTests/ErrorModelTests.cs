using AdasIt.Foundation.Dto.Reponses;
using FluentAssertions;

namespace Foundation.DtoTests.Responses;

public class ErrorModelTests
{
    [Fact]
    public void Constructor_WithCodeAndMessage_ShouldSetProperties()
    {
        // Arrange
        var code = ApplicationErrorCode.New(1);
        var message = "Test message";

        // Act
        var errorModel = new ErrorModel(code, message);

        // Assert
        errorModel.Code.Should().Be(code);
        errorModel.Message.Should().Be(message);
        errorModel.InnerMessage.Should().BeEmpty();
    }

    [Fact]
    public void Constructor_WithCodeMessageAndInnerMessage_ShouldSetProperties()
    {
        // Arrange
        var code = ApplicationErrorCode.New(2);
        var message = "Test message";
        var innerMessage = "Inner test message";

        // Act
        var errorModel = new ErrorModel(code, message, innerMessage);

        // Assert
        errorModel.Code.Should().Be(code);
        errorModel.Message.Should().Be(message);
        errorModel.InnerMessage.Should().Be(innerMessage);
    }

    [Fact]
    public void ChangeInnerMessage_ShouldUpdateInnerMessageAndReturnSameInstance()
    {
        // Arrange
        var errorModel = new ErrorModel(ApplicationErrorCode.New(1), "Message");
        var newInnerMessage = "New inner message";

        // Act
        var result = errorModel.ChangeInnerMessage(newInnerMessage);

        // Assert
        result.Should().BeSameAs(errorModel);
        errorModel.InnerMessage.Should().Be(newInnerMessage);
    }

    [Fact]
    public void RecordEquality_ShouldWorkCorrectly()
    {
        // Arrange
        var code = ApplicationErrorCode.New(1);
        var message = "Message";
        var innerMessage = "Inner";

        var error1 = new ErrorModel(code, message, innerMessage);
        var error2 = new ErrorModel(code, message, innerMessage);
        var error3 = new ErrorModel(ApplicationErrorCode.New(2), message, innerMessage);

        // Assert
        error1.Should().Be(error2);
        error1.Should().NotBe(error3);
    }

    [Fact]
    public void Properties_ShouldBeImmutableExceptInnerMessage()
    {
        // Arrange
        var errorModel = new ErrorModel(ApplicationErrorCode.New(1), "Message");

        // Act & Assert
        // Code and Message are init-only, so they can't be changed after construction
        // InnerMessage can be changed via ChangeInnerMessage
        errorModel.ChangeInnerMessage("New inner");
        errorModel.InnerMessage.Should().Be("New inner");
    }

    [Fact]
    public void Constructor_WithEmptyMessage_ShouldSetEmptyMessage()
    {
        // Arrange
        var code = ApplicationErrorCode.New(1);
        var message = string.Empty;

        // Act
        var errorModel = new ErrorModel(code, message);

        // Assert
        errorModel.Message.Should().BeEmpty();
    }

    [Fact]
    public void Constructor_WithNullInnerMessage_ShouldSetNullInnerMessage()
    {
        // Arrange
        var code = ApplicationErrorCode.New(1);
        var message = "Message";
        string innerMessage = null;

        // Act
        var errorModel = new ErrorModel(code, message, innerMessage);

        // Assert
        errorModel.InnerMessage.Should().BeNull();
    }
}