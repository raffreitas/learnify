using System.ComponentModel.DataAnnotations;

namespace Learnify.Courses.Infrastructure.Persistence.Settings;

public sealed record DatabaseSettings
{
    public const string SectionName = nameof(DatabaseSettings);

    [Required] public required string ConnectionString { get; init; }
}