using Learnify.Courses.Domain.Aggregates.Courses.Entities;

namespace Learnify.Courses.Application.Courses.DTOs;

public sealed record ModuleDto
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required int Order { get; init; }
    public required LessonDto[] Lessons { get; init; } = [];

    internal static ModuleDto FromModule(Module module) => new()
    {
        Id = module.Id,
        Title = module.Title,
        Order = module.Order,
        Lessons = module.Lessons.Select(LessonDto.FromLesson).ToArray(),
    };
}