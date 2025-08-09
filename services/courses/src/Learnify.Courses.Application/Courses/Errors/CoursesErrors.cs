using Learnify.Courses.Application.Shared.Errors;

namespace Learnify.Courses.Application.Courses.Errors;

public static class CoursesErrors
{
    public static NotFoundError CourseNotFound(Guid id) => new($"Course with id '{id}' not found.");
    public static DomainValidationError CourseCannotBeUpdated(string message) => new(message);
    public static ConflictError CourseAlreadyExists(string message) => new(message);
    public static DomainValidationError ModuleCannotBeAdded(string message) => new(message);
    public static ConflictError ModuleAlreadyExists => new("Module with the same title already exists");

    public static NotFoundError ModuleNotFound(Guid id) => new($"Module with id '{id}' not found.");
    public static NotFoundError LessonNotFound(Guid id) => new($"Lesson with id '{id}' not found.");
    public static DomainValidationError InvalidReorderPayload(string message) => new(message);
}