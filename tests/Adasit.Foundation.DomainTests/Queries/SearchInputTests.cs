using Adasit.Foundation.Domain.Queries;
using FluentAssertions;

namespace Adasit.Foundation.DomainTests.Queries;

public class SearchInputTests
{
    #region Constructor and Default Values Tests

    [Fact]
    public void Constructor_ShouldInitializeWithDefaultValues()
    {
        // Act
        var searchInput = new SearchInput();

        // Assert
        searchInput.Page.Should().Be(0);
        searchInput.PerPage.Should().Be(0);
        searchInput.Search.Should().BeNull();
        searchInput.OrderBy.Should().BeNull();
        searchInput.Order.Should().Be(SearchOrder.Undefined);
    }

    [Fact]
    public void Constructor_ShouldAllowObjectInitializer()
    {
        // Act
        var searchInput = new SearchInput
        {
            Page = 5,
            PerPage = 20,
            Search = "test",
            OrderBy = "Name",
            Order = SearchOrder.Desc
        };

        // Assert
        searchInput.Page.Should().Be(5);
        searchInput.PerPage.Should().Be(20);
        searchInput.Search.Should().Be("test");
        searchInput.OrderBy.Should().Be("Name");
        searchInput.Order.Should().Be(SearchOrder.Desc);
    }

    #endregion

    #region Normalize Method - Page Tests

    [Fact]
    public void Normalize_WhenPageIsNegative_ShouldSetToZero()
    {
        // Arrange
        var searchInput = new SearchInput { Page = -1 };

        // Act
        searchInput.Normalize();

        // Assert
        searchInput.Page.Should().Be(0);
    }

    [Fact]
    public void Normalize_WhenPageIsZero_ShouldRemainZero()
    {
        // Arrange
        var searchInput = new SearchInput { Page = 0 };

        // Act
        searchInput.Normalize();

        // Assert
        searchInput.Page.Should().Be(0);
    }

    [Fact]
    public void Normalize_WhenPageIsPositive_ShouldRemainUnchanged()
    {
        // Arrange
        var searchInput = new SearchInput { Page = 5 };

        // Act
        searchInput.Normalize();

        // Assert
        searchInput.Page.Should().Be(5);
    }

    [Theory]
    [InlineData(-100)]
    [InlineData(-50)]
    [InlineData(-1)]
    public void Normalize_WhenPageIsVariousNegativeValues_ShouldSetToZero(int negativePage)
    {
        // Arrange
        var searchInput = new SearchInput { Page = negativePage };

        // Act
        searchInput.Normalize();

        // Assert
        searchInput.Page.Should().Be(0);
    }

    #endregion

    #region Normalize Method - PerPage Tests

    [Fact]
    public void Normalize_WhenPerPageIsZero_ShouldSetToTen()
    {
        // Arrange
        var searchInput = new SearchInput { PerPage = 0 };

        // Act
        searchInput.Normalize();

        // Assert
        searchInput.PerPage.Should().Be(10);
    }

    [Fact]
    public void Normalize_WhenPerPageIsNegative_ShouldSetToTen()
    {
        // Arrange
        var searchInput = new SearchInput { PerPage = -5 };

        // Act
        searchInput.Normalize();

        // Assert
        searchInput.PerPage.Should().Be(10);
    }

    [Fact]
    public void Normalize_WhenPerPageIsPositive_ShouldRemainUnchanged()
    {
        // Arrange
        var searchInput = new SearchInput { PerPage = 25 };

        // Act
        searchInput.Normalize();

        // Assert
        searchInput.PerPage.Should().Be(25);
    }

    [Theory]
    [InlineData(-100)]
    [InlineData(-1)]
    [InlineData(0)]
    public void Normalize_WhenPerPageIsZeroOrNegative_ShouldSetToTen(int invalidPerPage)
    {
        // Arrange
        var searchInput = new SearchInput { PerPage = invalidPerPage };

        // Act
        searchInput.Normalize();

        // Assert
        searchInput.PerPage.Should().Be(10);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(50)]
    [InlineData(100)]
    public void Normalize_WhenPerPageIsPositive_ShouldNotChange(int validPerPage)
    {
        // Arrange
        var searchInput = new SearchInput { PerPage = validPerPage };

        // Act
        searchInput.Normalize();

        // Assert
        searchInput.PerPage.Should().Be(validPerPage);
    }

    #endregion

    #region Normalize Method - Order Tests

    [Fact]
    public void Normalize_WhenOrderIsUndefined_ShouldSetToAsc()
    {
        // Arrange
        var searchInput = new SearchInput { Order = SearchOrder.Undefined };

        // Act
        searchInput.Normalize();

        // Assert
        searchInput.Order.Should().Be(SearchOrder.Asc);
    }

    [Fact]
    public void Normalize_WhenOrderIsAsc_ShouldRemainAsc()
    {
        // Arrange
        var searchInput = new SearchInput { Order = SearchOrder.Asc };

        // Act
        searchInput.Normalize();

        // Assert
        searchInput.Order.Should().Be(SearchOrder.Asc);
    }

    [Fact]
    public void Normalize_WhenOrderIsDesc_ShouldRemainDesc()
    {
        // Arrange
        var searchInput = new SearchInput { Order = SearchOrder.Desc };

        // Act
        searchInput.Normalize();

        // Assert
        searchInput.Order.Should().Be(SearchOrder.Desc);
    }

    #endregion

    #region Normalize Method - Combined Tests

    [Fact]
    public void Normalize_WhenAllValuesNeedNormalization_ShouldNormalizeAll()
    {
        // Arrange
        var searchInput = new SearchInput
        {
            Page = -5,
            PerPage = -10,
            Order = SearchOrder.Undefined
        };

        // Act
        searchInput.Normalize();

        // Assert
        searchInput.Page.Should().Be(0);
        searchInput.PerPage.Should().Be(10);
        searchInput.Order.Should().Be(SearchOrder.Asc);
    }

    [Fact]
    public void Normalize_WhenNoValuesNeedNormalization_ShouldNotChange()
    {
        // Arrange
        var searchInput = new SearchInput
        {
            Page = 3,
            PerPage = 25,
            Search = "query",
            OrderBy = "CreatedDate",
            Order = SearchOrder.Desc
        };

        // Act
        searchInput.Normalize();

        // Assert
        searchInput.Page.Should().Be(3);
        searchInput.PerPage.Should().Be(25);
        searchInput.Search.Should().Be("query");
        searchInput.OrderBy.Should().Be("CreatedDate");
        searchInput.Order.Should().Be(SearchOrder.Desc);
    }

    [Fact]
    public void Normalize_ShouldNotModifySearchAndOrderByProperties()
    {
        // Arrange
        var searchInput = new SearchInput
        {
            Page = -1,
            PerPage = 0,
            Search = "test search",
            OrderBy = "PropertyName",
            Order = SearchOrder.Undefined
        };

        // Act
        searchInput.Normalize();

        // Assert
        searchInput.Search.Should().Be("test search");
        searchInput.OrderBy.Should().Be("PropertyName");
    }

    [Fact]
    public void Normalize_WhenCalledMultipleTimes_ShouldBeIdempotent()
    {
        // Arrange
        var searchInput = new SearchInput
        {
            Page = -5,
            PerPage = 0,
            Order = SearchOrder.Undefined
        };

        // Act
        searchInput.Normalize();
        var firstPage = searchInput.Page;
        var firstPerPage = searchInput.PerPage;
        var firstOrder = searchInput.Order;

        searchInput.Normalize();
        searchInput.Normalize();

        // Assert
        searchInput.Page.Should().Be(firstPage).And.Be(0);
        searchInput.PerPage.Should().Be(firstPerPage).And.Be(10);
        searchInput.Order.Should().Be(firstOrder).And.Be(SearchOrder.Asc);
    }

    #endregion

    #region Property Setter Tests

    [Fact]
    public void SetPage_ShouldUpdateValue()
    {
        // Arrange
        var searchInput = new SearchInput();

        // Act
        searchInput.Page = 10;

        // Assert
        searchInput.Page.Should().Be(10);
    }

    [Fact]
    public void SetPerPage_ShouldUpdateValue()
    {
        // Arrange
        var searchInput = new SearchInput();

        // Act
        searchInput.PerPage = 50;

        // Assert
        searchInput.PerPage.Should().Be(50);
    }

    [Fact]
    public void SetSearch_ShouldUpdateValue()
    {
        // Arrange
        var searchInput = new SearchInput();

        // Act
        searchInput.Search = "new search term";

        // Assert
        searchInput.Search.Should().Be("new search term");
    }

    [Fact]
    public void SetOrderBy_ShouldUpdateValue()
    {
        // Arrange
        var searchInput = new SearchInput();

        // Act
        searchInput.OrderBy = "UpdatedAt";

        // Assert
        searchInput.OrderBy.Should().Be("UpdatedAt");
    }

    [Fact]
    public void SetOrder_ShouldUpdateValue()
    {
        // Arrange
        var searchInput = new SearchInput();

        // Act
        searchInput.Order = SearchOrder.Desc;

        // Assert
        searchInput.Order.Should().Be(SearchOrder.Desc);
    }

    #endregion

    #region Record Equality Tests

    [Fact]
    public void Equality_WhenAllPropertiesAreEqual_ShouldBeEqual()
    {
        // Arrange
        var input1 = new SearchInput
        {
            Page = 1,
            PerPage = 10,
            Search = "test",
            OrderBy = "Name",
            Order = SearchOrder.Asc
        };

        var input2 = new SearchInput
        {
            Page = 1,
            PerPage = 10,
            Search = "test",
            OrderBy = "Name",
            Order = SearchOrder.Asc
        };

        // Act & Assert
        input1.Should().Be(input2);
        input1.Equals(input2).Should().BeTrue();
        (input1 == input2).Should().BeTrue();
    }

    [Fact]
    public void Equality_WhenPageDiffers_ShouldNotBeEqual()
    {
        // Arrange
        var input1 = new SearchInput { Page = 1 };
        var input2 = new SearchInput { Page = 2 };

        // Act & Assert
        input1.Should().NotBe(input2);
        (input1 != input2).Should().BeTrue();
    }

    [Fact]
    public void With_ShouldCreateCopyWithModifiedPage()
    {
        // Arrange
        var original = new SearchInput
        {
            Page = 1,
            PerPage = 10,
            Search = "test",
            OrderBy = "Name",
            Order = SearchOrder.Asc
        };

        // Act
        var modified = original with { Page = 5 };

        // Assert
        modified.Page.Should().Be(5);
        modified.PerPage.Should().Be(original.PerPage);
        modified.Search.Should().Be(original.Search);
        modified.OrderBy.Should().Be(original.OrderBy);
        modified.Order.Should().Be(original.Order);
    }

    #endregion

    #region Edge Cases and Boundary Tests

    [Fact]
    public void Normalize_WhenPageIsMaxInt_ShouldNotOverflow()
    {
        // Arrange
        var searchInput = new SearchInput { Page = int.MaxValue };

        // Act
        searchInput.Normalize();

        // Assert
        searchInput.Page.Should().Be(int.MaxValue);
    }

    [Fact]
    public void Normalize_WhenPerPageIsMaxInt_ShouldNotOverflow()
    {
        // Arrange
        var searchInput = new SearchInput { PerPage = int.MaxValue };

        // Act
        searchInput.Normalize();

        // Assert
        searchInput.PerPage.Should().Be(int.MaxValue);
    }

    [Fact]
    public void Search_WhenSetToEmptyString_ShouldNotBeNull()
    {
        // Arrange
        var searchInput = new SearchInput { Search = string.Empty };

        // Act
        searchInput.Normalize();

        // Assert
        searchInput.Search.Should().NotBeNull();
        searchInput.Search.Should().BeEmpty();
    }

    #endregion
}