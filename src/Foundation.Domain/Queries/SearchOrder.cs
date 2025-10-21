namespace Adasit.Foundation.Domain.Queries;

/// <summary>
/// Specifies the sort order direction for search results.
/// </summary>
public enum SearchOrder
{
    /// <summary>
    /// The sort order is not defined. This value should be normalized before use.
    /// </summary>
    /// <remarks>
    /// When using <see cref="SearchInput.Normalize"/>, this value will be converted to <see cref="Asc"/>.
    /// </remarks>
    Undefined,

    /// <summary>
    /// Sort results in ascending order (A-Z, 0-9, oldest to newest).
    /// </summary>
    Asc,

    /// <summary>
    /// Sort results in descending order (Z-A, 9-0, newest to oldest).
    /// </summary>
    Desc
}
