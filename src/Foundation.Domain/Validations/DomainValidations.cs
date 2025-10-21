using Adasit.Foundation.Domain.ValuesObjects;
using System.Runtime.CompilerServices;

namespace Adasit.Foundation.Domain.Validations;

/// <summary>
/// Provides extension methods for domain validation that return <see cref="Notification"/> objects.
/// </summary>
public static class DomainValidations
{
    /// <summary>
    /// Validates that the target object is not null.
    /// </summary>
    /// <param name="target">The object to validate.</param>
    /// <param name="fieldName">The name of the field being validated (automatically populated by the compiler).</param>
    /// <returns>A <see cref="Notification"/> if the validation fails, otherwise null.</returns>
    public static Notification? NotNull(this object target,
                              [CallerArgumentExpression(nameof(target))] string fieldName = "")
    {
        Notification? notification = null;

        if (target is null)
        {
            var message = DefaultsErrorsMessages.NotNull.GetMessage(fieldName);
            notification = new Notification(fieldName, message, CommonErrorCodes.Validation);
        }

        return notification;
    }

    /// <summary>
    /// Validates that the target Guid is not empty.
    /// </summary>
    /// <param name="target">The Guid to validate.</param>
    /// <param name="fieldName">The name of the field being validated (automatically populated by the compiler).</param>
    /// <returns>A <see cref="Notification"/> if the validation fails, otherwise null.</returns>
    public static Notification? NotNull(this Guid target,
                              [CallerArgumentExpression(nameof(target))] string fieldName = "")
    {
        Notification? notification = null;

        if (target == Guid.Empty)
        {
            var message = DefaultsErrorsMessages.NotNull.GetMessage(fieldName);
            notification = new Notification(fieldName, message, CommonErrorCodes.Validation);
        }

        return notification;
    }

    /// <summary>
    /// Validates that the target string is not null, empty, or consists only of white-space characters.
    /// </summary>
    /// <param name="target">The string to validate.</param>
    /// <param name="fieldName">The name of the field being validated (automatically populated by the compiler).</param>
    /// <returns>A <see cref="Notification"/> if the validation fails, otherwise null.</returns>
    public static Notification? NotNullOrEmptyOrWhiteSpace(this string? target,
                              [CallerArgumentExpression(nameof(target))] string fieldName = "")
    {
        Notification? notification = null;

        if (string.IsNullOrWhiteSpace(target) || string.IsNullOrEmpty(target))
        {
            var message = DefaultsErrorsMessages.NotNull.GetMessage(fieldName);
            notification = new Notification(fieldName, message, CommonErrorCodes.Validation);
        }

        return notification;
    }

    /// <summary>
    /// Validates that the target DateTime is not the default value.
    /// </summary>
    /// <param name="target">The DateTime to validate.</param>
    /// <param name="fieldName">The name of the field being validated (automatically populated by the compiler).</param>
    /// <returns>A <see cref="Notification"/> if the validation fails, otherwise null.</returns>
    public static Notification? NotDefaultDateTime(this DateTime target,
                              [CallerArgumentExpression(nameof(target))] string fieldName = "")
    {
        DateTime? nullableTarget = target;

        var notification = nullableTarget.NotDefaultDateTime(fieldName);

        return notification;
    }

    /// <summary>
    /// Validates that the target nullable DateTime is not the default value (only if it has a value).
    /// </summary>
    /// <param name="target">The nullable DateTime to validate.</param>
    /// <param name="fieldName">The name of the field being validated (automatically populated by the compiler).</param>
    /// <returns>A <see cref="Notification"/> if the validation fails, otherwise null.</returns>
    public static Notification? NotDefaultDateTime(this DateTime? target,
                          [CallerArgumentExpression(nameof(target))] string fieldName = "")
    {
        Notification? notification = null;

        if (target.HasValue && target.Value == default)
        {
            var message = DefaultsErrorsMessages.NotDefaultDateTime.GetMessage(fieldName);
            notification = new Notification(fieldName, message, CommonErrorCodes.Validation);
        }

        return notification;
    }

    /// <summary>
    /// Validates that the target string length is between the specified minimum and maximum lengths.
    /// </summary>
    /// <param name="target">The string to validate.</param>
    /// <param name="minLength">The minimum allowed length.</param>
    /// <param name="maxLength">The maximum allowed length.</param>
    /// <param name="fieldName">The name of the field being validated (automatically populated by the compiler).</param>
    /// <returns>A <see cref="Notification"/> if the validation fails, otherwise null.</returns>
    public static Notification? BetweenLength(this string? target, int minLength, int maxLength,
                              [CallerArgumentExpression(nameof(target))] string fieldName = "")
    {
        Notification? notification = null;

        if (!string.IsNullOrEmpty(target) && (target.Length < minLength || target.Length > maxLength))
        {
            var message = DefaultsErrorsMessages.BetweenLength.GetMessage(fieldName, minLength, maxLength);
            notification = new Notification(fieldName, message, CommonErrorCodes.Validation);
        }

        return notification;
    }

    /// <summary>
    /// Validates that the target string is a valid absolute URL.
    /// </summary>
    /// <param name="target">The string to validate as a URL.</param>
    /// <param name="fieldName">The name of the field being validated (automatically populated by the compiler).</param>
    /// <returns>A <see cref="Notification"/> if the validation fails, otherwise null.</returns>
    public static Notification? ValidUrl(this string target,
                              [CallerArgumentExpression(nameof(target))] string fieldName = "")
    {
        Notification? notification = null;

        if (!string.IsNullOrEmpty(target) && !Uri.TryCreate(target, UriKind.Absolute, out _))
        {
            var message = DefaultsErrorsMessages.InvalidUrl.GetMessage(fieldName);
            notification = new Notification(fieldName, message, CommonErrorCodes.Validation);
        }

        return notification;
    }
}
