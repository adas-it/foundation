using Adasit.Foundation.Domain.ValuesObjects;
using FluentAssertions;

namespace Adasit.Foundation.DomainTests.ValuesObjects;

public class DomainErrorCodeTests
{
    #region New Method Tests

    [Fact]
    public void New_WhenCalledWithValue_ShouldCreateDomainErrorCode()
    {
        // Arrange
        var errorValue = 1000;

        // Act
        var errorCode = DomainErrorCode.New(errorValue);

        // Assert
        errorCode.Should().NotBeNull();
        ((int)errorCode).Should().Be(errorValue);
    }

    [Fact]
    public void New_WhenCalledWithZero_ShouldCreateDomainErrorCode()
    {
        // Arrange
        var errorValue = 0;

        // Act
        var errorCode = DomainErrorCode.New(errorValue);

        // Assert
        ((int)errorCode).Should().Be(errorValue);
    }

    [Fact]
    public void New_WhenCalledWithNegativeValue_ShouldCreateDomainErrorCode()
    {
        // Arrange
        var errorValue = -1;

        // Act
        var errorCode = DomainErrorCode.New(errorValue);

        // Assert
        ((int)errorCode).Should().Be(errorValue);
    }

    [Fact]
    public void New_WhenCalledWithMaxInt_ShouldCreateDomainErrorCode()
    {
        // Arrange
        var errorValue = int.MaxValue;

        // Act
        var errorCode = DomainErrorCode.New(errorValue);

        // Assert
        ((int)errorCode).Should().Be(errorValue);
    }

    #endregion

    #region ToString Tests

    [Fact]
    public void ToString_ShouldReturnValueAsString()
    {
        // Arrange
        var errorCode = DomainErrorCode.New(1002);

        // Act
        var result = errorCode.ToString();

        // Assert
        result.Should().Be("1002");
    }

    [Fact]
    public void ToString_WithZero_ShouldReturnZero()
    {
        // Arrange
        var errorCode = DomainErrorCode.New(0);

        // Act
        var result = errorCode.ToString();

        // Assert
        result.Should().Be("0");
    }

    #endregion

    #region Implicit Conversion Tests

    [Fact]
    public void ImplicitConversion_ToInt_ShouldReturnValue()
    {
        // Arrange
        var errorCode = DomainErrorCode.New(5000);

        // Act
        int result = errorCode;

        // Assert
        result.Should().Be(5000);
    }

    [Fact]
    public void ImplicitConversion_ToInt_ShouldAllowDirectComparison()
    {
        // Arrange
        var errorCode = DomainErrorCode.New(1001);

        // Act & Assert
        (errorCode == 1001).Should().BeTrue();
        (errorCode != 1001).Should().BeFalse();
        (errorCode > 1000).Should().BeTrue();
        (errorCode < 2000).Should().BeTrue();
    }

    #endregion

    #region Record Equality Tests

    [Fact]
    public void Equality_WhenValuesAreEqual_ShouldBeEqual()
    {
        // Arrange
        var errorCode1 = DomainErrorCode.New(100);
        var errorCode2 = DomainErrorCode.New(100);

        // Act & Assert
        errorCode1.Should().Be(errorCode2);
        (errorCode1 == errorCode2).Should().BeTrue();
        errorCode1.Equals(errorCode2).Should().BeTrue();
    }

    [Fact]
    public void Equality_WhenValuesAreDifferent_ShouldNotBeEqual()
    {
        // Arrange
        var errorCode1 = DomainErrorCode.New(100);
        var errorCode2 = DomainErrorCode.New(200);

        // Act & Assert
        errorCode1.Should().NotBe(errorCode2);
        (errorCode1 != errorCode2).Should().BeTrue();
        errorCode1.Equals(errorCode2).Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_WhenValuesAreEqual_ShouldReturnSameHashCode()
    {
        // Arrange
        var errorCode1 = DomainErrorCode.New(500);
        var errorCode2 = DomainErrorCode.New(500);

        // Act & Assert
        errorCode1.GetHashCode().Should().Be(errorCode2.GetHashCode());
    }

    #endregion
}