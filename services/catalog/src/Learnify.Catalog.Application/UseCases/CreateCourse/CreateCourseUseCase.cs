using FluentResults;

using Learnify.Catalog.Core.Entities;
using Learnify.Catalog.Core.Enums;
using Learnify.Catalog.Core.Repositories;

namespace Learnify.Catalog.Application.UseCases.CreateCourse;

internal class CreateCourseUseCase(ICourseRepository courseRepository) : ICreateCourseUseCase
{
    public async Task<Result> ExecuteAsync(CreateCourseRequest request, CancellationToken cancellationToken = default)
    {
        // TODO: Validar se o curso já existe
        // Ir até o MS de course para buscar o restante das informações do courso
        // Ir até o MS de course para buscar o restante das informações do instrutor

        var course = new Course
        {
            Id = request.CourseId,
            Title = request.Title,
            Description = request.Description,
            Price = request.Price,
            Currency = request.Currency,
            ImageUrl = request.ImageUrl,
            Language = request.Language,
            DifficultyLevel = Enum.Parse<DifficultyLevel>(request.DifficultyLevel, true),
            InstructorId = request.InstructorId,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow,
            Categories = [],
            Modules = [],
            Instructor = new Instructor
            {
                Id = Guid.CreateVersion7(),
                Bio = "",
                FullName = "John Doe",
                ImageUrl = "https://example.com/instructor.jpg",
            },
            IsListed = false
        };

        await courseRepository.AddAsync(course, cancellationToken);

        return Result.Ok();
    }
}