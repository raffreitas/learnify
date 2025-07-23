using Learnify.Courses.Domain.Exceptions;
using Learnify.Courses.Domain.SeedWork;

namespace Learnify.Courses.Domain.Aggregates.Courses.ValueObjects;

public readonly record struct Price : IValueObject
{
    private Price(decimal value, string currency = "BRL")
    {
        Value = value;
        Currency = currency;
    }

    public decimal Value { get; }
    public string Currency { get; }

    public static Price Create(decimal value, string currency = "BRL")
    {
        DomainException.ThrowIfNegative(value, nameof(value));
        return new Price(value, currency);
    }

    public override string ToString() => Value.ToString("C2");
}
