using Learnify.Courses.Application.Courses.UseCases.UpdateModule;

namespace Learnify.Courses.WebApi.Models;

public sealed class UpdateModuleModel
{
    public required string Title { get; init; }
    public required int Order { get; init; }

    public UpdateModuleRequest ToRequest(Guid courseId, Guid moduleId) => new()
    {
        CourseId = courseId,
        ModuleId = moduleId,
        Title = Title,
        Order = Order
    };
}
