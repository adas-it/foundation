namespace Adasit.Foundation.Domain.Queries;

/// <summary>
/// Represents the output result of a paginated search operation.
/// </summary>
/// <typeparam name="TAggregate">The type of aggregate root or entity being returned in the search results.</typeparam>
/// <param name="CurrentPage">The zero-based index of the current page.</param>
/// <param name="PerPage">The number of items per page.</param>
/// <param name="Total">The total number of items across all pages.</param>
/// <param name="Items">The collection of items for the current page.</param>
/// <remarks>
/// This record is typically used in conjunction with <see cref="SearchInput"/> to provide
/// a complete pagination solution. It contains both the requested page of data and metadata
/// about the total result set.
/// </remarks>
/// <example>
/// <code>
/// var searchOutput = new SearchOutput&lt;Product&gt;(
///     CurrentPage: 0,
///     PerPage: 10,
///     Total: 100,
///     Items: products
/// );
/// 
/// // Total pages = Math.Ceiling(100.0 / 10) = 10 pages
/// </code>
/// </example>
public record SearchOutput<TAggregate>(
    int CurrentPage,
    int PerPage,
    int Total,
    ICollection<TAggregate> Items
);
