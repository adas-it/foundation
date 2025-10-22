using Adasit.Foundation.Domain.ValuesObjects;
using FluentAssertions;

namespace Adasit.Foundation.DomainTests.ValuesObjects;

public class NameTests
{
    #region Constructor - Valid Values Tests

    [Fact]
    public void Constructor_WhenValueIsValid_ShouldCreateName()
    {
        // Arrange
        var value = "John Doe";

        // Act
        Name name = value;

        // Assert
        name.Should().NotBeNull();
        name.Value.Should().Be(value);
    }

    [Fact]
    public void Constructor_WhenValueHasWhitespace_ShouldTrimValue()
    {
        // Arrange
        var value = "  Jane Smith  ";

        // Act
        Name name = value;

        // Assert
        name.Value.Should().Be("Jane Smith");
    }

    [Fact]
    public void Constructor_WhenValueIsAtMinLength_ShouldCreateName()
    {
        // Arrange
        var value = "Bob"; // Length = 3

        // Act
        Name name = value;

        // Assert
        name.Value.Should().Be(value);
    }

    [Fact]
    public void Constructor_WhenValueIsAtMaxLength_ShouldCreateName()
    {
        // Arrange
        var value = new string('A', Name.MaxLength); // Length = 70

        // Act
        Name name = value;

        // Assert
        name.Value.Should().Be(value);
        name.Value.Length.Should().Be(70);
    }

    [Theory]
    [InlineData("Alice")]
    [InlineData("Bob Smith")]
    [InlineData("María José García")]
    [InlineData("O'Brien")]
    [InlineData("Jean-Pierre")]
    public void Constructor_WithVariousValidNames_ShouldCreateName(string validName)
    {
        // Act
        Name name = validName;

        // Assert
        name.Value.Should().Be(validName);
    }

    #endregion

    #region Constructor - Invalid Values Tests

    [Fact]
    public void Constructor_WhenValueBelowMinLength_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var value = "AB"; // Length = 2

        // Act
        var act = () => { Name name = value; };

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("value")
            .WithMessage("*Name must be between 3 and 70 characters*");
    }

    [Fact]
    public void Constructor_WhenValueAboveMaxLength_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var value = new string('A', Name.MaxLength + 1); // Length = 71

        // Act
        var act = () => { Name name = value; };

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Name must be between 3 and 70 characters*");
    }

    [Fact]
    public void Constructor_WhenValueIsNull_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => { Name name = value!; };

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Constructor_WhenValueIsEmpty_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var value = string.Empty;

        // Act
        var act = () => { Name name = value; };

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Constructor_WhenValueIsWhitespaceOnly_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var value = "   "; // Trims to empty

        // Act
        var act = () => { Name name = value; };

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    #endregion

    #region Implicit Conversion Tests

    [Fact]
    public void ImplicitConversion_FromString_ShouldCreateName()
    {
        // Arrange
        var value = "Alice";

        // Act
        Name name = value;

        // Assert
        name.Value.Should().Be(value);
    }

    [Fact]
    public void ImplicitConversion_ToString_ShouldReturnValue()
    {
        // Arrange
        Name name = "Charlie";

        // Act
        string? result = name;

        // Assert
        result.Should().Be("Charlie");
    }

    [Fact]
    public void ImplicitConversion_NullNameToString_ShouldReturnNull()
    {
        // Arrange
        Name? name = null;

        // Act
        string? result = name;

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void ImplicitConversion_ShouldAllowFluentUsage()
    {
        // Arrange
        Name name = "David";

        // Act
        string upperName = ((string)name).ToUpper();

        // Assert
        upperName.Should().Be("DAVID");
    }

    #endregion

    #region Constants Tests

    [Fact]
    public void MinLength_ShouldBe3()
    {
        // Assert
        Name.MinLength.Should().Be(3);
    }

    [Fact]
    public void MaxLength_ShouldBe70()
    {
        // Assert
        Name.MaxLength.Should().Be(70);
    }

    #endregion

    #region ToString Tests

    [Fact]
    public void ToString_ShouldReturnValue()
    {
        // Arrange
        Name name = "David";

        // Act
        var result = name.ToString();

        // Assert
        result.Should().Be("David");
    }

    [Fact]
    public void ToString_AfterTrimming_ShouldReturnTrimmedValue()
    {
        // Arrange
        Name name = "  Emma  ";

        // Act
        var result = name.ToString();

        // Assert
        result.Should().Be("Emma");
    }

    #endregion

    #region Record Equality Tests

    [Fact]
    public void Equality_WhenNamesHaveSameValue_ShouldBeEqual()
    {
        // Arrange
        Name name1 = "Emma";
        Name name2 = "Emma";

        // Act & Assert
        name1.Should().Be(name2);
        (name1 == name2).Should().BeTrue();
        name1.Equals(name2).Should().BeTrue();
    }

    [Fact]
    public void Equality_WhenNamesHaveDifferentValues_ShouldNotBeEqual()
    {
        // Arrange
        Name name1 = "Frank";
        Name name2 = "Grace";

        // Act & Assert
        name1.Should().NotBe(name2);
        (name1 != name2).Should().BeTrue();
        name1.Equals(name2).Should().BeFalse();
    }

    [Fact]
    public void Equality_IsCaseSensitive()
    {
        // Arrange
        Name name1 = "Alice";
        Name name2 = "alice";

        // Act & Assert
        name1.Should().NotBe(name2);
    }

    [Fact]
    public void GetHashCode_WhenEqual_ShouldReturnSameHashCode()
    {
        // Arrange
        Name name1 = "Henry";
        Name name2 = "Henry";

        // Act & Assert
        name1.GetHashCode().Should().Be(name2.GetHashCode());
    }

    #endregion
}