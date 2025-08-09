using System.ComponentModel.DataAnnotations;

namespace Learnify.Courses.Infrastructure.Storage.Settings;

public sealed record StorageSettings
{
    public const string SectionName = nameof(StorageSettings);

    [Required] public required string Endpoint { get; init; }

    [Required] public required string AccessKey { get; init; }

    [Required] public required string SecretKey { get; init; }

    [Required] public required bool UseSsl { get; init; }
};