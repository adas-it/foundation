namespace Adasit.Foundation.Domain.Queries;

/// <summary>
/// Represents the input parameters for search and pagination operations in query scenarios.
/// This record provides filtering, sorting, and pagination capabilities for data retrieval.
/// </summary>
/// <remarks>
/// The <see cref="SearchInput"/> is designed to be used with <see cref="SearchOutput{TAggregate}"/>
/// to provide a complete pagination and search solution. Call <see cref="Normalize"/> to ensure
/// all values are within valid ranges before processing.
/// </remarks>
public record SearchInput
{
    /// <summary>
    /// Gets or sets the zero-based page number for pagination.
    /// </summary>
    /// <value>The current page number. Negative values will be normalized to 0.</value>
    /// <remarks>
    /// This is a zero-based index, where 0 represents the first page.
    /// Use <see cref="Normalize"/> to ensure the value is non-negative.
    /// </remarks>
    public int Page { get; set; }

    /// <summary>
    /// Gets or sets the number of items to return per page.
    /// </summary>
    /// <value>The number of items per page. Values less than or equal to 0 will be normalized to 10.</value>
    /// <remarks>
    /// The default value after normalization is 10 items per page.
    /// Use <see cref="Normalize"/> to ensure the value is positive.
    /// </remarks>
    public int PerPage { get; set; }

    /// <summary>
    /// Gets or sets the search term for filtering results.
    /// </summary>
    /// <value>The search string, or null if no search filter is applied.</value>
    /// <remarks>
    /// The interpretation of this search term is implementation-specific and typically
    /// used for full-text search or filtering across multiple fields.
    /// </remarks>
    public string? Search { get; set; }

    /// <summary>
    /// Gets or sets the name of the field to sort results by.
    /// </summary>
    /// <value>The field name for sorting, or null if no specific ordering is required.</value>
    /// <remarks>
    /// The field name should correspond to a property on the entity being queried.
    /// The sort direction is controlled by the <see cref="Order"/> property.
    /// </remarks>
    public string? OrderBy { get; set; }

    /// <summary>
    /// Gets or sets the sort order direction.
    /// </summary>
    /// <value>The sort order. Defaults to <see cref="SearchOrder.Asc"/> after normalization if set to <see cref="SearchOrder.Undefined"/>.</value>
    /// <remarks>
    /// Use <see cref="Normalize"/> to ensure undefined sort orders are set to a default value.
    /// </remarks>
    public SearchOrder Order { get; set; }

    /// <summary>
    /// Normalizes the search input values to ensure they are within valid ranges.
    /// </summary>
    /// <remarks>
    /// This method performs the following normalizations:
    /// <list type="bullet">
    /// <item><description>Sets <see cref="Page"/> to 0 if it's negative.</description></item>
    /// <item><description>Sets <see cref="PerPage"/> to 10 if it's less than or equal to 0.</description></item>
    /// <item><description>Sets <see cref="Order"/> to <see cref="SearchOrder.Asc"/> if it's <see cref="SearchOrder.Undefined"/>.</description></item>
    /// </list>
    /// Call this method before using the search input to ensure consistent behavior.
    /// </remarks>
    /// <example>
    /// <code>
    /// var searchInput = new SearchInput 
    /// { 
    ///     Page = -1, 
    ///     PerPage = 0, 
    ///     Order = SearchOrder.Undefined 
    /// };
    /// 
    /// searchInput.Normalize();
    /// 
    /// // After normalization:
    /// // searchInput.Page = 0
    /// // searchInput.PerPage = 10
    /// // searchInput.Order = SearchOrder.Asc
    /// </code>
    /// </example>
    public void Normalize()
    {
        if (Page < 0) Page = 0;
        if (PerPage <= 0) PerPage = 10;
        if (Order == SearchOrder.Undefined) Order = SearchOrder.Asc;
    }
}
