﻿namespace Learnify.Users.Domain.Entities.ValueObjects;

public record Name
{
    public string FirstName { get; init; }
    public string LastName { get; init; }

    private Name(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public static Name Create(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be null or empty", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be null or empty", nameof(lastName));

        return new Name(firstName, lastName);
    }
}
