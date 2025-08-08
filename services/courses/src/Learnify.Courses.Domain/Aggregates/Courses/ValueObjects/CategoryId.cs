using Learnify.Courses.Domain.Exceptions;
using Learnify.Courses.Domain.SeedWork;

namespace Learnify.Courses.Domain.Aggregates.Courses.ValueObjects;

public sealed record CategoryId : IValueObject
{
    public Guid Value { get; private init; }

    private CategoryId(Guid value)
    {
        Value = value;
    }

    public static CategoryId Create(Guid value)
    {
        if (value == Guid.Empty)
            DomainException.ThrowIfEmpty(value, nameof(value));

        return new CategoryId(value);
    }
}