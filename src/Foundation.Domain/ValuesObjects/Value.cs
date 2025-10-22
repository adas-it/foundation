namespace Adasit.Foundation.Domain.ValuesObjects;

/// <summary>
/// Represents a general-purpose string value object with flexible length constraints.
/// This value object is suitable for storing various types of textual data such as configuration values,
/// metadata, notes, or any string content that requires validation between 1 and 2500 characters.
/// </summary>
/// <remarks>
/// <para>
/// The <see cref="Value"/> class inherits from <see cref="StringValueObject"/> and provides:
/// <list type="bullet">
/// <item><description>Automatic whitespace trimming</description></item>
/// <item><description>Length validation (1-2500 characters)</description></item>
/// <item><description>Immutability through record semantics</description></item>
/// <item><description>Implicit conversions between string and Value</description></item>
/// </list>
/// </para>
/// <para>
/// This value object is ideal for scenarios where you need validated string content
/// with generous length limits, such as:
/// <list type="bullet">
/// <item><description>Application settings and configuration values</description></item>
/// <item><description>User-generated content (comments, notes, messages)</description></item>
/// <item><description>Metadata and custom attributes</description></item>
/// <item><description>Long-form text fields that exceed typical name/description limits</description></item>
/// </list>
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // Creating a Value instance using implicit conversion
/// Value configValue = "Server timeout: 30 seconds";
/// 
/// // Accessing the underlying string
/// string setting = configValue.Value;
/// 
/// // Implicit conversion back to string
/// string settingString = configValue;
/// 
/// // Value with whitespace is automatically trimmed
/// Value trimmedValue = "  Spaces will be removed  ";
/// Console.WriteLine(trimmedValue.Value); // Output: "Spaces will be removed"
/// 
/// // Using in a domain entity
/// public class Configuration
/// {
///     public required Value Key { get; init; }
///     public required Value Content { get; init; }
/// }
/// 
/// var config = new Configuration
/// {
///     Key = "MaxRetries",
///     Content = "5"
/// };
/// </code>
/// </example>
/// <seealso cref="Name"/>
/// <seealso cref="Description"/>
/// <seealso cref="StringValueObject"/>
public record Value : StringValueObject
{
    /// <summary>
    /// The minimum allowed length for a <see cref="Value"/> instance.
    /// </summary>
    /// <value>1 character</value>
    /// <remarks>
    /// This ensures that Value instances cannot be empty after trimming whitespace.
    /// </remarks>
    public const int MinLength = 1;

    /// <summary>
    /// The maximum allowed length for a <see cref="Value"/> instance.
    /// </summary>
    /// <value>2500 characters</value>
    /// <remarks>
    /// This generous limit allows for storing substantial textual content while still
    /// maintaining reasonable database and memory constraints.
    /// </remarks>
    public const int MaxLength = 2500;

    /// <summary>
    /// Initializes a new instance of the <see cref="Value"/> class.
    /// </summary>
    /// <param name="value">
    /// The string value to encapsulate. Must be between <see cref="MinLength"/> and <see cref="MaxLength"/> 
    /// characters after trimming whitespace.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="value"/> is null, empty, or its length (after trimming) 
    /// is less than <see cref="MinLength"/> or greater than <see cref="MaxLength"/>.
    /// </exception>
    /// <remarks>
    /// This constructor is private to enforce the use of implicit conversion operators,
    /// providing a more natural syntax when working with Value objects.
    /// The base class automatically trims leading and trailing whitespace before validation.
    /// </remarks>
    private Value(string value) : base(value, MinLength, MaxLength, nameof(Value)) { }

    /// <summary>
    /// Implicitly converts a <see cref="string"/> to a <see cref="Value"/> instance.
    /// </summary>
    /// <param name="value">The string value to convert.</param>
    /// <returns>A new <see cref="Value"/> instance containing the validated and trimmed string.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="value"/> does not meet the length requirements.
    /// </exception>
    /// <remarks>
    /// This operator enables natural syntax when assigning strings to Value properties:
    /// <code>Value myValue = "some text";</code>
    /// The conversion automatically triggers validation and trimming through the constructor.
    /// </remarks>
    /// <example>
    /// <code>
    /// // Implicit conversion from string to Value
    /// Value content = "Configuration setting";
    /// 
    /// // Can be used in assignments
    /// public class Settings
    /// {
    ///     public Value ApiKey { get; set; }
    /// }
    /// 
    /// var settings = new Settings { ApiKey = "abc123xyz" };
    /// </code>
    /// </example>
    public static implicit operator Value(string value) => new(value);

    /// <summary>
    /// Implicitly converts a <see cref="Value"/> instance to a <see cref="string"/>.
    /// </summary>
    /// <param name="symbol">The <see cref="Value"/> instance to convert. Can be null.</param>
    /// <returns>
    /// The underlying string value from the <see cref="StringValueObject.Value"/> property,
    /// or <c>null</c> if <paramref name="symbol"/> is <c>null</c>.
    /// </returns>
    /// <remarks>
    /// This operator enables seamless integration with APIs expecting strings:
    /// <code>string text = myValue;</code>
    /// The parameter is named 'symbol' to maintain consistency with other value object implementations.
    /// </remarks>
    /// <example>
    /// <code>
    /// Value myValue = "Hello, World!";
    /// 
    /// // Implicit conversion to string
    /// string text = myValue;
    /// Console.WriteLine(text); // Output: Hello, World!
    /// 
    /// // Null-safe conversion
    /// Value? nullValue = null;
    /// string? nullText = nullValue; // nullText is null
    /// 
    /// // Using with string methods
    /// Value content = "test";
    /// bool startsWith = ((string)content).StartsWith("t"); // true
    /// </code>
    /// </example>
    public static implicit operator string?(Value? symbol) => symbol?.Value;
}
