using Learnify.Courses.Application.Courses.UseCases.ReorderModules;

namespace Learnify.Courses.WebApi.Models;

public sealed class ReorderModulesModel
{
    // map of ModuleId -> Order
    public required Dictionary<Guid, int> Positions { get; init; }

    public ReorderModulesRequest ToRequest(Guid courseId) => new()
    {
        CourseId = courseId,
        Positions = Positions
    };
}
