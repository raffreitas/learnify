using Learnify.Courses.Application.Abstractions;
using Learnify.Courses.Domain.Aggregates.Courses;
using Learnify.Courses.Domain.Aggregates.Courses.Repositories;

namespace Learnify.Courses.Application.Courses.UseCases.CreateCourse;

internal sealed class CreateCourseUseCase(ICourseRepository courseRepository, IUnitOfWork unitOfWork)
    : ICreateCourseUseCase
{
    public async Task<CreateCourseResponse> ExecuteAsync(
        CreateCourseRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = request.Validate();
        // TODO: Use ResultPattern for better error handling 
        if (!validationResult.IsValid)
            throw new InvalidOperationException(validationResult.Errors[0].ErrorMessage);

        Guid instructorId = new("018e3dd4-58aa-77e3-b663-8d14fcb672c1");
        var course = Course.CreateAsDraft(instructorId, request.Title);

        await courseRepository.AddAsync(course, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return new CreateCourseResponse(course.Id);
    }
}