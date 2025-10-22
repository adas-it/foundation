using Adasit.Foundation.Domain.ValuesObjects;
using FluentAssertions;

namespace Adasit.Foundation.DomainTests.ValuesObjects;

public class StringValueObjectTests
{
    #region Test Helper Class

    // Concrete implementation for testing the abstract base class
    private record TestStringValueObject : StringValueObject
    {
        public const int MinLength = 3;
        public const int MaxLength = 10;

        public TestStringValueObject(string value)
            : base(value, MinLength, MaxLength, nameof(TestStringValueObject))
        {
        }
    }

    #endregion

    #region Constructor - Valid Values Tests

    [Fact]
    public void Constructor_WhenValueIsValid_ShouldCreateInstance()
    {
        // Arrange
        var value = "valid";

        // Act
        var result = new TestStringValueObject(value);

        // Assert
        result.Should().NotBeNull();
        result.Value.Should().Be(value);
    }

    [Fact]
    public void Constructor_WhenValueHasWhitespace_ShouldTrimValue()
    {
        // Arrange
        var value = "  trimmed  ";

        // Act
        var result = new TestStringValueObject(value);

        // Assert
        result.Value.Should().Be("trimmed");
    }

    [Fact]
    public void Constructor_WhenValueIsAtMinLength_ShouldCreateInstance()
    {
        // Arrange
        var value = "abc"; // Length = 3

        // Act
        var result = new TestStringValueObject(value);

        // Assert
        result.Value.Should().Be(value);
    }

    [Fact]
    public void Constructor_WhenValueIsAtMaxLength_ShouldCreateInstance()
    {
        // Arrange
        var value = "abcdefghij"; // Length = 10

        // Act
        var result = new TestStringValueObject(value);

        // Assert
        result.Value.Should().Be(value);
    }

    [Fact]
    public void Constructor_WhenValueWithinRange_ShouldCreateInstance()
    {
        // Arrange
        var value = "middle"; // Length = 6

        // Act
        var result = new TestStringValueObject(value);

        // Assert
        result.Value.Should().Be(value);
    }

    #endregion

    #region Constructor - Invalid Values Tests

    [Fact]
    public void Constructor_WhenValueIsNull_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => new TestStringValueObject(value!);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*TestStringValueObject must be between 3 and 10 characters*");
    }

    [Fact]
    public void Constructor_WhenValueIsEmpty_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var value = string.Empty;

        // Act
        var act = () => new TestStringValueObject(value);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*TestStringValueObject must be between 3 and 10 characters*");
    }

    [Fact]
    public void Constructor_WhenValueBelowMinLength_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var value = "ab"; // Length = 2

        // Act
        var act = () => new TestStringValueObject(value);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("value")
            .WithMessage("*TestStringValueObject must be between 3 and 10 characters*");
    }

    [Fact]
    public void Constructor_WhenValueAboveMaxLength_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var value = "abcdefghijk"; // Length = 11

        // Act
        var act = () => new TestStringValueObject(value);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("value")
            .WithMessage("*TestStringValueObject must be between 3 and 10 characters*");
    }

    [Fact]
    public void Constructor_WhenWhitespaceOnlyBelowMinLength_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var value = "   "; // Trims to empty string

        // Act
        var act = () => new TestStringValueObject(value);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*TestStringValueObject must be between 3 and 10 characters*");
    }

    #endregion

    #region ToString Tests

    [Fact]
    public void ToString_ShouldReturnValue()
    {
        // Arrange
        var value = "test";
        var obj = new TestStringValueObject(value);

        // Act
        var result = obj.ToString();

        // Assert
        result.Should().Be(value);
    }

    [Fact]
    public void ToString_AfterTrimming_ShouldReturnTrimmedValue()
    {
        // Arrange
        var obj = new TestStringValueObject("  trimmed  ");

        // Act
        var result = obj.ToString();

        // Assert
        result.Should().Be("trimmed");
    }

    #endregion

    #region Record Equality Tests

    [Fact]
    public void Equality_WhenValuesAreEqual_ShouldBeEqual()
    {
        // Arrange
        var obj1 = new TestStringValueObject("same");
        var obj2 = new TestStringValueObject("same");

        // Act & Assert
        obj1.Should().Be(obj2);
        (obj1 == obj2).Should().BeTrue();
        obj1.Equals(obj2).Should().BeTrue();
    }

    [Fact]
    public void Equality_WhenValuesAreDifferent_ShouldNotBeEqual()
    {
        // Arrange
        var obj1 = new TestStringValueObject("first");
        var obj2 = new TestStringValueObject("second");

        // Act & Assert
        obj1.Should().NotBe(obj2);
        (obj1 != obj2).Should().BeTrue();
        obj1.Equals(obj2).Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_WhenValuesAreEqual_ShouldReturnSameHashCode()
    {
        // Arrange
        var obj1 = new TestStringValueObject("same");
        var obj2 = new TestStringValueObject("same");

        // Act & Assert
        obj1.GetHashCode().Should().Be(obj2.GetHashCode());
    }

    #endregion
}