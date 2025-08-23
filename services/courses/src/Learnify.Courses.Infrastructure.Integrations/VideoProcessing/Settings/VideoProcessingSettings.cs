using System.ComponentModel.DataAnnotations;

namespace Learnify.Courses.Infrastructure.Integrations.VideoProcessing.Settings;

internal sealed record VideoProcessingSettings
{
    public const string SectionName = nameof(VideoProcessingSettings);

    [Required, Url] public required string BaseUrl { get; init; }
}