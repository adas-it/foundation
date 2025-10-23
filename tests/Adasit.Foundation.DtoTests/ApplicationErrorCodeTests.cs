using AdasIt.Foundation.Dto.Reponses;
using FluentAssertions;

namespace Foundation.DtoTests.Responses;

public class ApplicationErrorCodeTests
{
    [Fact]
    public void New_ShouldCreateApplicationErrorCodeWithValue()
    {
        // Arrange
        var value = 42;

        // Act
        var errorCode = ApplicationErrorCode.New(value);

        // Assert
        errorCode.Value.Should().Be(value);
    }

    [Fact]
    public void ImplicitOperator_FromInt_ShouldCreateApplicationErrorCode()
    {
        // Arrange
        var value = 123;

        // Act
        ApplicationErrorCode errorCode = value;

        // Assert
        errorCode.Value.Should().Be(value);
    }

    [Fact]
    public void ImplicitOperator_ToInt_ShouldReturnValue()
    {
        // Arrange
        var value = 456;
        var errorCode = ApplicationErrorCode.New(value);

        // Act
        int result = errorCode;

        // Assert
        result.Should().Be(value);
    }

    [Fact]
    public void ToString_ShouldReturnValueAsString()
    {
        // Arrange
        var value = 789;
        var errorCode = ApplicationErrorCode.New(value);

        // Act
        var result = errorCode.ToString();

        // Assert
        result.Should().Be(value.ToString());
    }

    [Fact]
    public void CompareTo_ShouldCompareValues()
    {
        // Arrange
        var code1 = ApplicationErrorCode.New(10);
        var code2 = ApplicationErrorCode.New(20);
        var code3 = ApplicationErrorCode.New(10);

        // Act & Assert
        code1.CompareTo(code2).Should().BeLessThan(0);
        code2.CompareTo(code1).Should().BeGreaterThan(0);
        code1.CompareTo(code3).Should().Be(0);
    }

    [Fact]
    public void CompareTo_WithNull_ShouldReturnPositive()
    {
        // Arrange
        var code = ApplicationErrorCode.New(5);

        // Act
        var result = code.CompareTo(null);

        // Assert
        result.Should().BeGreaterThan(0);
    }

    [Fact]
    public void RecordEquality_ShouldWorkCorrectly()
    {
        // Arrange
        var code1 = ApplicationErrorCode.New(100);
        var code2 = ApplicationErrorCode.New(100);
        var code3 = ApplicationErrorCode.New(200);

        // Assert
        code1.Should().Be(code2);
        code1.Should().NotBe(code3);
    }

    [Fact]
    public void Value_ShouldBeImmutable()
    {
        // Arrange
        var code = ApplicationErrorCode.New(50);

        // Assert
        code.Value.Should().Be(50);
        // Value is init-only, so it can't be changed
    }

    [Fact]
    public void New_WithZero_ShouldCreateCodeWithZero()
    {
        // Act
        var code = ApplicationErrorCode.New(0);

        // Assert
        code.Value.Should().Be(0);
    }

    [Fact]
    public void New_WithNegativeValue_ShouldCreateCodeWithNegativeValue()
    {
        // Act
        var code = ApplicationErrorCode.New(-1);

        // Assert
        code.Value.Should().Be(-1);
    }
}