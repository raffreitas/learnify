namespace Learnify.Courses.Application.Courses.UseCases.CreateCourse;

public interface ICreateCourseUseCase
{
    Task<CreateCourseResponse> ExecuteAsync(CreateCourseRequest request, CancellationToken cancellationToken = default);
}