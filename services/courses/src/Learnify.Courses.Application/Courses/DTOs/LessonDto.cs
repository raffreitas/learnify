using Learnify.Courses.Domain.Aggregates.Courses.Entities;

namespace Learnify.Courses.Application.Courses.DTOs;

public sealed record LessonDto
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string VideoUrl { get; init; }
    public required int Order { get; init; }
    public required bool IsPublic { get; init; }

    internal static LessonDto FromLesson(Lesson lesson) => new()
    {
        Id = lesson.Id,
        Title = lesson.Title,
        Order = lesson.Order,
        Description = lesson.Description,
        IsPublic = lesson.IsPublic,
        VideoUrl = lesson.VideoUrl,
    };
}