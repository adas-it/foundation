using System.Reflection;

namespace Adasit.Foundation.Domain;

/// <summary>
/// Represents a base class for creating type-safe enumerations with associated keys.
/// </summary>
/// <typeparam name="TKey">The type of the key associated with each enumeration value.</typeparam>
public abstract record Enumeration<TKey>
{
    /// <summary>
    /// Gets the display name of the enumeration value.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Gets the unique key identifier for the enumeration value.
    /// </summary>
    public TKey Key { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Enumeration{TKey}"/> class.
    /// </summary>
    /// <param name="id">The unique key identifier for the enumeration value.</param>
    /// <param name="name">The display name of the enumeration value.</param>
    protected Enumeration(TKey id, string name) => (Key, Name) = (id, name);

    /// <summary>
    /// Returns the display name of the enumeration value.
    /// </summary>
    /// <returns>The name of the enumeration value.</returns>
    public sealed override string ToString() => Name;

    /// <summary>
    /// Retrieves all defined values of a specific enumeration type.
    /// </summary>
    /// <typeparam name="T">The enumeration type derived from <see cref="Enumeration{TKey}"/>.</typeparam>
    /// <returns>An enumerable collection of all defined enumeration values.</returns>
    public static IEnumerable<T> GetAll<T>() where T : Enumeration<TKey> =>
        typeof(T).GetFields(BindingFlags.Public |
                            BindingFlags.Static |
                            BindingFlags.DeclaredOnly)
            .Select(f => f.GetValue(null))
            .Cast<T>();

    /// <summary>
    /// Retrieves an enumeration value by its key.
    /// </summary>
    /// <typeparam name="T">The enumeration type derived from <see cref="Enumeration{TKey}"/>.</typeparam>
    /// <param name="key">The key of the enumeration value to retrieve.</param>
    /// <returns>The enumeration value with the specified key.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no enumeration value with the specified key is found.</exception>
    public static T GetByKey<T>(TKey key) where T : Enumeration<TKey>
    {
        return GetAll<T>().First(x => x.Key!.Equals(key));
    }
}
