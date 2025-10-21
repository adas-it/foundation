namespace Adasit.Foundation.Domain.Validations;

/// <summary>
/// Provides default error message templates for domain validations.
/// Messages use composite formatting with placeholders (e.g., {0}, {1}) for field names and validation parameters.
/// </summary>
public static class DefaultsErrorsMessages
{
    /// <summary>
    /// Error message template for required field validation.
    /// Format: "The field {0} is required."
    /// </summary>
    /// <example>
    /// <code>
    /// var message = DefaultsErrorsMessages.NotNull.GetMessage("Name");
    /// // Result: "The field Name is required."
    /// </code>
    /// </example>
    public static readonly string NotNull = "The field {0} is required.";

    /// <summary>
    /// Error message template for default DateTime validation.
    /// Format: "The date field {0} has to have valid data."
    /// </summary>
    /// <example>
    /// <code>
    /// var message = DefaultsErrorsMessages.NotDefaultDateTime.GetMessage("CreatedAt");
    /// // Result: "The date field CreatedAt has to have valid data."
    /// </code>
    /// </example>
    public static readonly string NotDefaultDateTime = "The date field {0} has to have valid data.";

    /// <summary>
    /// Error message template for string length validation.
    /// Format: "The field {0} has to have length between {1} and {2}."
    /// </summary>
    /// <example>
    /// <code>
    /// var message = DefaultsErrorsMessages.BetweenLength.GetMessage("Name", 3, 100);
    /// // Result: "The field Name has to have length between 3 and 100."
    /// </code>
    /// </example>
    public static readonly string BetweenLength = "The field {0} has to have length between {1} and {2}.";

    /// <summary>
    /// Error message template for date comparison validation.
    /// Format: "{0} has to be greater than {1}."
    /// </summary>
    /// <example>
    /// <code>
    /// var message = DefaultsErrorsMessages.Date0CannotBeBeforeDate1.GetMessage("EndDate", "StartDate");
    /// // Result: "EndDate has to be greater than StartDate."
    /// </code>
    /// </example>
    public static readonly string Date0CannotBeBeforeDate1 = "{0} has to be greater than {1}.";

    /// <summary>
    /// Error message template for URL validation.
    /// Format: "The field {0} has to have a valid Url."
    /// </summary>
    /// <example>
    /// <code>
    /// var message = DefaultsErrorsMessages.InvalidUrl.GetMessage("Website");
    /// // Result: "The field Website has to have a valid Url."
    /// </code>
    /// </example>
    public static readonly string InvalidUrl = "The field {0} has to have a valid Url.";

    /// <summary>
    /// Error message for invalid GUID validation.
    /// Format: "The value provided is not a valid GUID."
    /// </summary>
    public static readonly string InvalidGuid = "The value provided is not a valid GUID.";

    /// <summary>
    /// Formats an error message template with the provided parameters using composite formatting.
    /// </summary>
    /// <param name="msg">The message template containing placeholders (e.g., {0}, {1}).</param>
    /// <param name="par">The parameters to insert into the message template placeholders.</param>
    /// <returns>A formatted error message string with all placeholders replaced by the corresponding parameter values.</returns>
    /// <example>
    /// <code>
    /// var message = DefaultsErrorsMessages.BetweenLength.GetMessage("Name", 3, 100);
    /// // Result: "The field Name has to have length between 3 and 100."
    /// </code>
    /// </example>
    public static string GetMessage(this string msg, params object[] par)
    {
        return string.Format(msg, par);
    }
}
