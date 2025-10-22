using Adasit.Foundation.Domain.ValuesObjects;
using FluentAssertions;

namespace Adasit.Foundation.DomainTests.ValuesObjects;

public class DescriptionTests
{
    #region Constructor - Valid Values Tests

    [Fact]
    public void Constructor_WhenValueIsValid_ShouldCreateDescription()
    {
        // Arrange
        var value = "This is a valid description";

        // Act
        Description description = value;

        // Assert
        description.Should().NotBeNull();
        description.Value.Should().Be(value);
    }

    [Fact]
    public void Constructor_WhenValueHasWhitespace_ShouldTrimValue()
    {
        // Arrange
        var value = "  Description with spaces  ";

        // Act
        Description description = value;

        // Assert
        description.Value.Should().Be("Description with spaces");
    }

    [Fact]
    public void Constructor_WhenValueIsAtMinLength_ShouldCreateDescription()
    {
        // Arrange
        var value = "ABC"; // Length = 3

        // Act
        Description description = value;

        // Assert
        description.Value.Should().Be(value);
    }

    [Fact]
    public void Constructor_WhenValueIsAtMaxLength_ShouldCreateDescription()
    {
        // Arrange
        var value = new string('X', Description.MaxLength); // Length = 250

        // Act
        Description description = value;

        // Assert
        description.Value.Should().Be(value);
        description.Value.Length.Should().Be(250);
    }

    [Theory]
    [InlineData("Short description")]
    [InlineData("Medium length description with more details about the item")]
    [InlineData("A detailed description explaining all the features and benefits")]
    public void Constructor_WithVariousValidDescriptions_ShouldCreateDescription(string validDescription)
    {
        // Act
        Description description = validDescription;

        // Assert
        description.Value.Should().Be(validDescription);
    }

    #endregion

    #region Constructor - Invalid Values Tests

    [Fact]
    public void Constructor_WhenValueBelowMinLength_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var value = "AB"; // Length = 2

        // Act
        var act = () => { Description description = value; };

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("value")
            .WithMessage("*Description must be between 3 and 250 characters*");
    }

    [Fact]
    public void Constructor_WhenValueAboveMaxLength_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var value = new string('X', Description.MaxLength + 1); // Length = 251

        // Act
        var act = () => { Description description = value; };

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Description must be between 3 and 250 characters*");
    }

    [Fact]
    public void Constructor_WhenValueIsNull_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => { Description description = value!; };

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Constructor_WhenValueIsEmpty_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var value = string.Empty;

        // Act
        var act = () => { Description description = value; };

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Constructor_WhenValueIsWhitespaceOnly_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var value = "     ";

        // Act
        var act = () => { Description description = value; };

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    #endregion

    #region Implicit Conversion Tests

    [Fact]
    public void ImplicitConversion_FromString_ShouldCreateDescription()
    {
        // Arrange
        var value = "Product description";

        // Act
        Description description = value;

        // Assert
        description.Value.Should().Be(value);
    }

    [Fact]
    public void ImplicitConversion_ToString_ShouldReturnValue()
    {
        // Arrange
        Description description = "Service description";

        // Act
        string? result = description;

        // Assert
        result.Should().Be("Service description");
    }

    [Fact]
    public void ImplicitConversion_NullDescriptionToString_ShouldReturnNull()
    {
        // Arrange
        Description? description = null;

        // Act
        string? result = description;

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void ImplicitConversion_ShouldAllowFluentUsage()
    {
        // Arrange
        Description description = "Test description";

        // Act
        bool contains = ((string)description).Contains("Test");

        // Assert
        contains.Should().BeTrue();
    }

    #endregion

    #region Constants Tests

    [Fact]
    public void MinLength_ShouldBe3()
    {
        // Assert
        Description.MinLength.Should().Be(3);
    }

    [Fact]
    public void MaxLength_ShouldBe250()
    {
        // Assert
        Description.MaxLength.Should().Be(250);
    }

    #endregion

    #region ToString Tests

    [Fact]
    public void ToString_ShouldReturnValue()
    {
        // Arrange
        Description description = "Test description";

        // Act
        var result = description.ToString();

        // Assert
        result.Should().Be("Test description");
    }

    [Fact]
    public void ToString_AfterTrimming_ShouldReturnTrimmedValue()
    {
        // Arrange
        Description description = "  Trimmed description  ";

        // Act
        var result = description.ToString();

        // Assert
        result.Should().Be("Trimmed description");
    }

    #endregion

    #region Record Equality Tests

    [Fact]
    public void Equality_WhenDescriptionsHaveSameValue_ShouldBeEqual()
    {
        // Arrange
        Description desc1 = "Same description";
        Description desc2 = "Same description";

        // Act & Assert
        desc1.Should().Be(desc2);
        (desc1 == desc2).Should().BeTrue();
        desc1.Equals(desc2).Should().BeTrue();
    }

    [Fact]
    public void Equality_WhenDescriptionsHaveDifferentValues_ShouldNotBeEqual()
    {
        // Arrange
        Description desc1 = "First description";
        Description desc2 = "Second description";

        // Act & Assert
        desc1.Should().NotBe(desc2);
        (desc1 != desc2).Should().BeTrue();
        desc1.Equals(desc2).Should().BeFalse();
    }

    [Fact]
    public void Equality_IsCaseSensitive()
    {
        // Arrange
        Description desc1 = "Description";
        Description desc2 = "description";

        // Act & Assert
        desc1.Should().NotBe(desc2);
    }

    [Fact]
    public void GetHashCode_WhenEqual_ShouldReturnSameHashCode()
    {
        // Arrange
        Description desc1 = "Identical description";
        Description desc2 = "Identical description";

        // Act & Assert
        desc1.GetHashCode().Should().Be(desc2.GetHashCode());
    }

    #endregion
}