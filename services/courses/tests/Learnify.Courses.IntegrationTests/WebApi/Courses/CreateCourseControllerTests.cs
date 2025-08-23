using System.Net;
using System.Net.Http.Json;

using Learnify.Courses.Application.Courses.UseCases.CreateCourse;
using Learnify.Courses.IntegrationTests.Shared;

using Shouldly;

namespace Learnify.Courses.IntegrationTests.WebApi.Courses;

[Trait("IntegrationTests", "WebApi - Courses")]
public class CreateCourseControllerTests(CustomWebApplicationFactory factory)
    : BaseIntegrationTest(factory), IAsyncLifetime
{
    [Fact(DisplayName = nameof(CreateCourse_ShouldSuccess_WhenRequestIsValid), Skip = "Skipped")]
    public async Task CreateCourse_ShouldSuccess_WhenRequestIsValid()
    {
        // Arrange
        var createCourse = new CreateCourseRequest { Title = Faker.Commerce.ProductName() };

        // Act
        var httpResponse = await PostAsync("/api/courses/v1", createCourse);

        // Assert
        httpResponse.EnsureSuccessStatusCode();
        httpResponse.StatusCode.ShouldBe(HttpStatusCode.Created);

        var response = await httpResponse.Content.ReadFromJsonAsync<CreateCourseResponse>();
        response.ShouldNotBeNull();
        response.CourseId.ShouldNotBe(Guid.Empty);
    }

    [Fact(DisplayName = nameof(CreateCourse_ShouldReturnBadRequest_WhenRequestIsInvalid))]
    public async Task CreateCourse_ShouldReturnBadRequest_WhenRequestIsInvalid()
    {
        // Arrange
        var createCourse = new CreateCourseRequest { Title = string.Empty };

        // Act
        var httpResponse = await PostAsync("/api/courses/v1", createCourse);

        // Assert
        httpResponse.IsSuccessStatusCode.ShouldBeFalse();
        httpResponse.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    public Task InitializeAsync() => Task.CompletedTask;
    public async Task DisposeAsync() => await CleanUpDatabaseAsync();
}