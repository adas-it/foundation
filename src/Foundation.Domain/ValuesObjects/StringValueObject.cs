namespace Adasit.Foundation.Domain.ValuesObjects;

/// <summary>
/// Represents an abstract base record for string-based value objects in the domain.
/// This class provides automatic trimming and length validation for string values,
/// ensuring domain invariants are maintained at construction time.
/// </summary>
/// <remarks>
/// <para>
/// Value objects derived from this class are immutable records that encapsulate a string value
/// with enforced length constraints. Common examples include <see cref="Name"/>, <see cref="Description"/>, 
/// and <see cref="Value"/>.
/// </para>
/// <para>
/// The constructor automatically trims whitespace from input values and validates length constraints,
/// throwing an <see cref="ArgumentOutOfRangeException"/> if the value is invalid.
/// </para>
/// </remarks>
public abstract record StringValueObject
{
    /// <summary>
    /// Gets the validated and trimmed string value.
    /// </summary>
    /// <value>
    /// The string value that has been trimmed of leading and trailing whitespace
    /// and validated to be within the specified length constraints.
    /// </value>
    /// <remarks>
    /// This value is guaranteed to meet the length requirements specified during construction
    /// and will never be null (empty string is used for null inputs).
    /// </remarks>
    public string Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="StringValueObject"/> class with the specified value and constraints.
    /// </summary>
    /// <param name="value">
    /// The string value to encapsulate. Null values are converted to empty strings.
    /// Leading and trailing whitespace will be automatically trimmed.
    /// </param>
    /// <param name="minLength">The minimum allowed length for the value (inclusive).</param>
    /// <param name="maxLength">The maximum allowed length for the value (inclusive).</param>
    /// <param name="name">
    /// The name of the value object type, used in error messages to provide context.
    /// Typically this should be the name of the derived class (e.g., "Name", "Description").
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the trimmed value length is less than <paramref name="minLength"/> 
    /// or greater than <paramref name="maxLength"/>.
    /// </exception>
    /// <remarks>
    /// <para>
    /// The validation process performs the following steps:
    /// <list type="number">
    /// <item><description>Converts null to empty string</description></item>
    /// <item><description>Trims leading and trailing whitespace</description></item>
    /// <item><description>Validates the resulting length against min/max constraints</description></item>
    /// <item><description>Assigns the validated value to the <see cref="Value"/> property</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// // In a derived class:
    /// public record Name : StringValueObject
    /// {
    ///     public const int MinLength = 3;
    ///     public const int MaxLength = 70;
    ///     
    ///     private Name(string value) 
    ///         : base(value, MinLength, MaxLength, nameof(Name)) 
    ///     { }
    /// }
    /// 
    /// // Usage:
    /// var name = new Name("  John Doe  "); // Value will be "John Doe" (trimmed)
    /// // var invalid = new Name("AB"); // Throws ArgumentOutOfRangeException
    /// </code>
    /// </example>
    protected StringValueObject(string value, int minLength, int maxLength, string name)
    {
        value = value?.Trim() ?? string.Empty;

        if (value.Length < minLength || value.Length > maxLength)
            throw new ArgumentOutOfRangeException(nameof(value),
                $"{name} must be between {minLength} and {maxLength} characters.");

        Value = value;
    }

    /// <summary>
    /// Returns the string representation of this value object.
    /// </summary>
    /// <returns>The <see cref="Value"/> property as a string.</returns>
    /// <remarks>
    /// This method is sealed to ensure consistent string representation across all derived types.
    /// The returned value is always the validated and trimmed string contained in the <see cref="Value"/> property.
    /// </remarks>
    public sealed override string ToString() => Value;
}
