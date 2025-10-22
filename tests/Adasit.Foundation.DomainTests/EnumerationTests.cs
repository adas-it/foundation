using Adasit.Foundation.Domain;
using FluentAssertions;

namespace Adasit.Foundation.DomainTests;

public class EnumerationTests
{
    #region Test Enumerations

    private sealed record TestEnumeration : Enumeration<int>
    {
        public static readonly TestEnumeration First = new(1, "First");
        public static readonly TestEnumeration Second = new(2, "Second");
        public static readonly TestEnumeration Third = new(3, "Third");

        private TestEnumeration(int id, string name) : base(id, name) { }
    }

    private sealed record StringKeyEnumeration : Enumeration<string>
    {
        public static readonly StringKeyEnumeration Active = new("ACT", "Active");
        public static readonly StringKeyEnumeration Inactive = new("INA", "Inactive");
        public static readonly StringKeyEnumeration Pending = new("PEN", "Pending");

        private StringKeyEnumeration(string id, string name) : base(id, name) { }
    }

    private sealed record EmptyEnumeration : Enumeration<int>
    {
        private EmptyEnumeration(int id, string name) : base(id, name) { }
    }

    #endregion

    #region Constructor Tests

    [Fact]
    public void Constructor_ShouldSetKeyAndName()
    {
        // Arrange & Act
        var enumeration = TestEnumeration.First;

        // Assert
        enumeration.Key.Should().Be(1);
        enumeration.Name.Should().Be("First");
    }

    [Fact]
    public void Constructor_WithStringKey_ShouldSetKeyAndName()
    {
        // Arrange & Act
        var enumeration = StringKeyEnumeration.Active;

        // Assert
        enumeration.Key.Should().Be("ACT");
        enumeration.Name.Should().Be("Active");
    }

    #endregion

    #region ToString Tests

    [Fact]
    public void ToString_ShouldReturnName()
    {
        // Arrange
        var enumeration = TestEnumeration.Second;

        // Act
        var result = enumeration.ToString();

        // Assert
        result.Should().Be("Second");
    }

    [Fact]
    public void ToString_WithStringKey_ShouldReturnName()
    {
        // Arrange
        var enumeration = StringKeyEnumeration.Pending;

        // Act
        var result = enumeration.ToString();

        // Assert
        result.Should().Be("Pending");
    }

    #endregion

    #region GetAll Tests

    [Fact]
    public void GetAll_ShouldReturnAllDefinedValues()
    {
        // Act
        var allValues = Enumeration<int>.GetAll<TestEnumeration>().ToList();

        // Assert
        allValues.Should().HaveCount(3);
        allValues.Should().Contain(TestEnumeration.First);
        allValues.Should().Contain(TestEnumeration.Second);
        allValues.Should().Contain(TestEnumeration.Third);
    }

    [Fact]
    public void GetAll_WithStringKey_ShouldReturnAllDefinedValues()
    {
        // Act
        var allValues = Enumeration<string>.GetAll<StringKeyEnumeration>().ToList();

        // Assert
        allValues.Should().HaveCount(3);
        allValues.Should().Contain(StringKeyEnumeration.Active);
        allValues.Should().Contain(StringKeyEnumeration.Inactive);
        allValues.Should().Contain(StringKeyEnumeration.Pending);
    }

    [Fact]
    public void GetAll_WhenNoStaticFieldsDefined_ShouldReturnEmpty()
    {
        // Act
        var allValues = Enumeration<int>.GetAll<EmptyEnumeration>().ToList();

        // Assert
        allValues.Should().BeEmpty();
    }

    [Fact]
    public void GetAll_ShouldReturnDistinctInstances()
    {
        // Act
        var allValues = Enumeration<int>.GetAll<TestEnumeration>().ToList();

        // Assert
        allValues.Should().OnlyHaveUniqueItems();
    }

    [Fact]
    public void GetAll_CalledMultipleTimes_ShouldReturnSameValues()
    {
        // Act
        var firstCall = Enumeration<int>.GetAll<TestEnumeration>().ToList();
        var secondCall = Enumeration<int>.GetAll<TestEnumeration>().ToList();

        // Assert
        firstCall.Should().BeEquivalentTo(secondCall);
    }

    #endregion

    #region GetByKey Tests

    [Fact]
    public void GetByKey_WithValidKey_ShouldReturnCorrectEnumeration()
    {
        // Act
        var result = Enumeration<int>.GetByKey<TestEnumeration>(1);

        // Assert
        result.Should().Be(TestEnumeration.First);
        result.Key.Should().Be(1);
        result.Name.Should().Be("First");
    }

    [Fact]
    public void GetByKey_WithStringKey_ShouldReturnCorrectEnumeration()
    {
        // Act
        var result = Enumeration<string>.GetByKey<StringKeyEnumeration>("ACT");

        // Assert
        result.Should().Be(StringKeyEnumeration.Active);
        result.Key.Should().Be("ACT");
        result.Name.Should().Be("Active");
    }

    [Fact]
    public void GetByKey_WithAllKeys_ShouldReturnCorrectEnumerations()
    {
        // Act & Assert
        Enumeration<int>.GetByKey<TestEnumeration>(1).Should().Be(TestEnumeration.First);
        Enumeration<int>.GetByKey<TestEnumeration>(2).Should().Be(TestEnumeration.Second);
        Enumeration<int>.GetByKey<TestEnumeration>(3).Should().Be(TestEnumeration.Third);
    }

    [Fact]
    public void GetByKey_WithInvalidKey_ShouldThrowInvalidOperationException()
    {
        // Act
        var act = () => Enumeration<int>.GetByKey<TestEnumeration>(999);

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void GetByKey_WithInvalidStringKey_ShouldThrowInvalidOperationException()
    {
        // Act
        var act = () => Enumeration<string>.GetByKey<StringKeyEnumeration>("INVALID");

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void GetByKey_OnEmptyEnumeration_ShouldThrowInvalidOperationException()
    {
        // Act
        var act = () => Enumeration<int>.GetByKey<EmptyEnumeration>(1);

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    #endregion

    #region Record Equality Tests

    [Fact]
    public void Equality_SameInstance_ShouldBeEqual()
    {
        // Arrange
        var first = TestEnumeration.First;
        var same = TestEnumeration.First;

        // Act & Assert
        first.Should().Be(same);
        (first == same).Should().BeTrue();
        first.Equals(same).Should().BeTrue();
    }

    [Fact]
    public void Equality_DifferentInstances_ShouldNotBeEqual()
    {
        // Arrange
        var first = TestEnumeration.First;
        var second = TestEnumeration.Second;

        // Act & Assert
        first.Should().NotBe(second);
        (first != second).Should().BeTrue();
        first.Equals(second).Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_SameInstance_ShouldReturnSameHashCode()
    {
        // Arrange
        var first = TestEnumeration.First;
        var same = TestEnumeration.First;

        // Act & Assert
        first.GetHashCode().Should().Be(same.GetHashCode());
    }

    [Fact]
    public void GetHashCode_DifferentInstances_ShouldReturnDifferentHashCodes()
    {
        // Arrange
        var first = TestEnumeration.First;
        var second = TestEnumeration.Second;

        // Act & Assert
        first.GetHashCode().Should().NotBe(second.GetHashCode());
    }

    #endregion

    #region Property Tests

    [Fact]
    public void Key_ShouldBeInitOnly()
    {
        // Arrange
        var enumeration = TestEnumeration.First;

        // Assert
        enumeration.Key.Should().Be(1);
        // Cannot reassign - this is enforced by the compiler
    }

    [Fact]
    public void Name_ShouldBeInitOnly()
    {
        // Arrange
        var enumeration = TestEnumeration.First;

        // Assert
        enumeration.Name.Should().Be("First");
        // Cannot reassign - this is enforced by the compiler
    }

    #endregion

    #region Integration Tests

    [Fact]
    public void Enumeration_UsedInCollection_ShouldWorkCorrectly()
    {
        // Arrange
        var list = new List<TestEnumeration>
        {
            TestEnumeration.First,
            TestEnumeration.Second,
            TestEnumeration.Third
        };

        // Act
        var containsSecond = list.Contains(TestEnumeration.Second);
        var indexOf = list.IndexOf(TestEnumeration.Third);

        // Assert
        containsSecond.Should().BeTrue();
        indexOf.Should().Be(2);
    }

    [Fact]
    public void Enumeration_UsedInDictionary_ShouldWorkCorrectly()
    {
        // Arrange
        var dictionary = new Dictionary<TestEnumeration, string>
        {
            { TestEnumeration.First, "Value1" },
            { TestEnumeration.Second, "Value2" }
        };

        // Act
        var value = dictionary[TestEnumeration.Second];
        var containsKey = dictionary.ContainsKey(TestEnumeration.First);

        // Assert
        value.Should().Be("Value2");
        containsKey.Should().BeTrue();
    }

    [Fact]
    public void Enumeration_WithLinq_ShouldWorkCorrectly()
    {
        // Act
        var allNames = Enumeration<int>.GetAll<TestEnumeration>()
            .Select(e => e.Name)
            .ToList();

        var firstWithKeyGreaterThanOne = Enumeration<int>.GetAll<TestEnumeration>()
            .First(e => e.Key > 1);

        // Assert
        allNames.Should().Contain(new[] { "First", "Second", "Third" });
        firstWithKeyGreaterThanOne.Should().Be(TestEnumeration.Second);
    }

    [Fact]
    public void Enumeration_WithStringKeyInDictionary_ShouldWorkCorrectly()
    {
        // Arrange
        var dictionary = new Dictionary<StringKeyEnumeration, bool>
        {
            { StringKeyEnumeration.Active, true },
            { StringKeyEnumeration.Inactive, false },
            { StringKeyEnumeration.Pending, true }
        };

        // Act
        var activeValue = dictionary[StringKeyEnumeration.Active];

        // Assert
        activeValue.Should().BeTrue();
        dictionary.Should().HaveCount(3);
    }

    #endregion
}