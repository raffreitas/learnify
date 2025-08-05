namespace Learnify.Courses.Domain.Exceptions;

public class DomainException(string message) : Exception(message)
{
    public static void ThrowIfNullOrEmpty(string? value, string paramName)
    {
        if (string.IsNullOrEmpty(value))
            throw new DomainException($"{paramName} cannot be null or empty.");
    }

    public static void ThrowIfNullOrWhitespace(string? value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException($"{paramName} cannot be null or whitespace.");
    }

    public static void ThrowIfNegative(int value, string paramName)
    {
        if (value < 0)
            throw new DomainException($"{paramName} cannot be negative.");
    }

    public static void ThrowIfNegative(decimal value, string paramName)
    {
        if (value < 0)
            throw new DomainException($"{paramName} cannot be negative.");
    }
}