using System.ComponentModel.DataAnnotations;

namespace Learnify.Catalog.Infrastructure.Persistence.Settings;

internal sealed record DatabaseSettings
{
    public const string SectionName = nameof(DatabaseSettings);

    [Required, MinLength(1)] public required string ConnectionString { get; init; }

    [Required, MinLength(1)] public required string DatabaseName { get; init; }
}