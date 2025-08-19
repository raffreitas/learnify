using Learnify.Courses.Domain.Exceptions;
using Learnify.Courses.Domain.SeedWork;

namespace Learnify.Courses.Domain.Aggregates.Instructors.ValueObjects;

public sealed record Name : IValueObject
{
    public string FirstName { get; private init; }
    public string LastName { get; private init; }

    private Name(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public static Name Create(string firstName, string lastName)
    {
        DomainException.ThrowIfNullOrWhitespace(firstName, nameof(firstName));
        DomainException.ThrowIfNullOrWhitespace(lastName, nameof(lastName));

        return new Name(firstName, lastName);
    }

    public override string ToString() => $"{FirstName} {LastName}";
}