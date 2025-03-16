using System.Text.RegularExpressions;

namespace Learnify.Users.Domain.Entities.ValueObjects;

public partial record Email
{
    public string Value { get; init; }
    private Email(string value)
    {
        Value = value;
    }

    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email cannot be null or empty", nameof(value));

        if (!EmailRegex().IsMatch(value))
            throw new ArgumentException("Email is invalid", nameof(value));

        return new Email(value);
    }

    [GeneratedRegex(@"^[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?\.)+[a-zA-Z]{2,}$")]
    private static partial Regex EmailRegex();
}
