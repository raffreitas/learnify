using Learnify.Courses.Domain.Aggregates.Courses.Entities;

namespace Learnify.Courses.Domain.Aggregates.Courses.Models;

public sealed record LessonInfo(
    string Title,
    string Description,
    LessonMedia Media,
    int Order,
    bool IsPublic
);