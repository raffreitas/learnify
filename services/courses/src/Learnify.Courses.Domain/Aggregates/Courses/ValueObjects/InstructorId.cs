using Learnify.Courses.Domain.Exceptions;
using Learnify.Courses.Domain.SeedWork;

namespace Learnify.Courses.Domain.Aggregates.Courses.ValueObjects;

public record InstructorId : IValueObject
{
    public Guid Value { get; private init; }

    private InstructorId(Guid value)
    {
        Value = value;
    }

    public static InstructorId Create(Guid value)
    {
        DomainException.ThrowIfEmpty(value, nameof(value));
        return new InstructorId(value);
    }

    public static implicit operator Guid(InstructorId instructorId) => instructorId.Value;
};