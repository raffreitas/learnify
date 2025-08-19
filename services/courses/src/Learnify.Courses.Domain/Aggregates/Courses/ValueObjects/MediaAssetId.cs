using Learnify.Courses.Domain.Exceptions;
using Learnify.Courses.Domain.SeedWork;

namespace Learnify.Courses.Domain.Aggregates.Courses.ValueObjects;

public sealed record MediaAssetId : IValueObject
{
    public Guid Value { get; private init; }

    private MediaAssetId(Guid value)
    {
        Value = value;
    }

    public static MediaAssetId Create(Guid value)
    {
        DomainException.ThrowIfEmpty(value, nameof(value));
        return new MediaAssetId(value);
    }

    public static implicit operator Guid(MediaAssetId mediaAssetId) => mediaAssetId.Value;
};