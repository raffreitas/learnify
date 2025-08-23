namespace Learnify.Courses.Application.Courses.UseCases.CreateLesson;

public sealed record CreateLessonResponse(Guid LessonId, string UploadUrl, int UploadExpiresIn);
