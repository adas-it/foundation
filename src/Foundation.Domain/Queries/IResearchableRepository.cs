namespace Adasit.Foundation.Domain.Queries;

/// <summary>
/// Defines the contract for a query repository that supports searching and retrieving entities in a CQRS (Command Query Responsibility Segregation) architecture.
/// This interface is part of the query side and focuses exclusively on read operations with search and pagination capabilities.
/// </summary>
/// <typeparam name="TEntity">The type of entity being queried.</typeparam>
/// <typeparam name="TEntityId">The type of the entity's identifier.</typeparam>
/// <remarks>
/// <para>
/// In CQRS architecture, this interface represents the query/read side of the pattern, complementing write-side repositories 
/// like <see cref="SeedWork.Repositories.ICommandRepository{TEntity, TEntityId}"/>. Implementations should be optimized for read performance 
/// and may query from denormalized read models or projections.
/// </para>
/// <para>
/// The repository provides two main operations:
/// <list type="bullet">
/// <item><description>Single entity retrieval by ID</description></item>
/// <item><description>Advanced search with filtering, sorting, and pagination</description></item>
/// </list>
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ProductQueryRepository : ISearchableRepository&lt;Product, ProductId&gt;
/// {
///     public async Task&lt;Product?&gt; GetByIdAsync(ProductId id, CancellationToken cancellationToken)
///     {
///         // Retrieve single product from read model
///     }
///     
///     public async Task&lt;SearchOutput&lt;Product&gt;&gt; SearchAsync&lt;TSearchInput&gt;(
///         TSearchInput input, 
///         CancellationToken cancellationToken)
///     {
///         // Execute search with pagination
///     }
/// }
/// </code>
/// </example>
public interface ISearchableRepository<TEntity, TEntityId>
{
    /// <summary>
    /// Retrieves a single entity by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to retrieve.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> that represents the asynchronous operation. 
    /// The task result contains the entity if found, or <c>null</c> if no entity exists with the specified identifier.
    /// </returns>
    /// <remarks>
    /// This method provides direct access to a single entity without the overhead of search operations.
    /// It is optimized for scenarios where the exact identifier is known.
    /// Implementations should handle cases where the entity doesn't exist by returning <c>null</c> rather than throwing exceptions.
    /// </remarks>
    /// <example>
    /// <code>
    /// var productId = ProductId.Load(Guid.Parse("12345678-1234-1234-1234-123456789012"));
    /// var product = await repository.GetByIdAsync(productId, cancellationToken);
    /// 
    /// if (product != null)
    /// {
    ///     // Process found product
    /// }
    /// </code>
    /// </example>
    Task<TEntity?> GetByIdAsync(TEntityId id, CancellationToken cancellationToken);

    /// <summary>
    /// Performs an advanced search operation with filtering, sorting, and pagination capabilities.
    /// </summary>
    /// <typeparam name="TSearchInput">
    /// The type of search input, typically <see cref="SearchInput"/> or a derived type that provides 
    /// filtering, sorting, and pagination parameters.
    /// </typeparam>
    /// <param name="input">
    /// The search input containing pagination parameters (page, per page), optional search terms, 
    /// sorting criteria (order by field, order direction), and any additional filter parameters.
    /// </param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> that represents the asynchronous operation.
    /// The task result contains a <see cref="SearchOutput{TEntity}"/> with the paginated results, 
    /// including the current page of items and metadata (total count, page info).
    /// </returns>
    /// <remarks>
    /// <para>
    /// This method supports complex query scenarios including:
    /// <list type="bullet">
    /// <item><description>Full-text search across entity fields</description></item>
    /// <item><description>Sorting by specified fields in ascending or descending order</description></item>
    /// <item><description>Pagination with configurable page size</description></item>
    /// <item><description>Custom filtering through derived search input types</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// Implementations should:
    /// <list type="bullet">
    /// <item><description>Normalize the search input before processing (e.g., using <see cref="SearchInput.Normalize"/>)</description></item>
    /// <item><description>Return an empty collection (not null) when no results are found</description></item>
    /// <item><description>Include accurate total count for pagination metadata</description></item>
    /// <item><description>Apply security filters as appropriate (e.g., tenant isolation, user permissions)</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// // Basic search with pagination
    /// var input = new SearchInput 
    /// { 
    ///     Page = 0, 
    ///     PerPage = 20,
    ///     Search = "laptop",
    ///     OrderBy = "Price",
    ///     Order = SearchOrder.Desc
    /// };
    /// 
    /// input.Normalize();
    /// 
    /// var result = await repository.SearchAsync(input, cancellationToken);
    /// 
    /// Console.WriteLine($"Found {result.Total} products");
    /// Console.WriteLine($"Showing page {result.CurrentPage + 1} of {Math.Ceiling(result.Total / (double)result.PerPage)}");
    /// 
    /// foreach (var product in result.Items)
    /// {
    ///     // Process each product in the page
    /// }
    /// </code>
    /// </example>
    /// <example>
    /// <code>
    /// // Advanced search with custom input type
    /// public record ProductSearchInput : SearchInput
    /// {
    ///     public decimal? MinPrice { get; set; }
    ///     public decimal? MaxPrice { get; set; }
    ///     public string? Category { get; set; }
    /// }
    /// 
    /// var advancedInput = new ProductSearchInput
    /// {
    ///     Page = 0,
    ///     PerPage = 10,
    ///     MinPrice = 100,
    ///     MaxPrice = 500,
    ///     Category = "Electronics",
    ///     OrderBy = "Name",
    ///     Order = SearchOrder.Asc
    /// };
    /// 
    /// var result = await repository.SearchAsync(advancedInput, cancellationToken);
    /// </code>
    /// </example>
    Task<SearchOutput<TEntity>> SearchAsync<TSearchInput>(TSearchInput input,
        CancellationToken cancellationToken);
}
