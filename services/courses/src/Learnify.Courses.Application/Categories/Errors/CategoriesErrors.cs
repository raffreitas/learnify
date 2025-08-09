using Learnify.Courses.Application.Shared.Errors;

namespace Learnify.Courses.Application.Categories.Errors;

public static class CategoriesErrors
{
    public static ConflictError CategoryAlreadyExists => new("Category already exists");
    public static NotFoundError CategoryNotFound(string message) => new(message);
}