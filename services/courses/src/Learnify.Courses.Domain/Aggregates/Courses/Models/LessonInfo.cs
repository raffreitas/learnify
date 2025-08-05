namespace Learnify.Courses.Domain.Aggregates.Courses.Models;

public sealed record LessonInfo(
    string Title,
    string Description,
    string VideoUrl,
    int Order,
    bool IsPublic
);