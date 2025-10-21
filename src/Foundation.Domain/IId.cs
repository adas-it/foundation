namespace Adasit.Foundation.Domain;

/// <summary>
/// Defines the contract for strongly-typed identifiers in the domain.
/// Strongly-typed IDs provide type safety and prevent mixing IDs of different entity types.
/// This interface uses covariance to allow for flexible inheritance hierarchies.
/// </summary>
/// <typeparam name="TSelf">The implementing type itself, enabling covariant return types.</typeparam>
/// <remarks>
/// Implementations of this interface should wrap a <see cref="Guid"/> value and provide factory methods
/// for creating new IDs and loading existing ones. This pattern ensures type safety while maintaining
/// the efficiency of Guid-based identifiers.
/// </remarks>
public interface IId<out TSelf> where TSelf : IId<TSelf>
{
    /// <summary>
    /// Creates a new instance of the identifier with a newly generated <see cref="Guid"/> value.
    /// </summary>
    /// <returns>A new instance of <typeparamref name="TSelf"/> with a unique identifier.</returns>
    /// <remarks>
    /// This method should generate a new <see cref="Guid"/> using <see cref="Guid.NewGuid()"/>
    /// to ensure uniqueness across the system.
    /// </remarks>
    static abstract TSelf New();

    /// <summary>
    /// Creates a new instance of the identifier from an existing <see cref="Guid"/> value.
    /// </summary>
    /// <param name="value">The <see cref="Guid"/> value to use for the identifier.</param>
    /// <returns>A new instance of <typeparamref name="TSelf"/> with the specified identifier value.</returns>
    /// <remarks>
    /// This method is used when loading entities from persistence or when working with existing identifiers.
    /// It allows reconstructing strongly-typed IDs from their underlying Guid values.
    /// </remarks>
    static abstract TSelf Load(Guid value);
}
