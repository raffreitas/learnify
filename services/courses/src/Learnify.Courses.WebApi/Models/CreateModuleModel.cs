using Learnify.Courses.Application.Courses.UseCases.CreateModule;

namespace Learnify.Courses.WebApi.Models;

public sealed class CreateModuleModel
{
    public required string Title { get; init; }
    public required int Order { get; init; }

    public CreateModuleRequest ToRequest(Guid courseId) => new() { Title = Title, CourseId = courseId, Order = Order };
}