using Learnify.Courses.Domain.Aggregates.Courses.ValueObjects;
using Learnify.Courses.Domain.Exceptions;

using Shouldly;

namespace Learnify.Courses.UnitTests.Domain.Aggregates.Courses.ValueObjects;

[Trait("UnitTests", "Domain - ValueObjects")]
public sealed class CategoryIdTests
{
    [Fact(DisplayName = nameof(Create_Should_Return_CategoryId))]
    public void Create_Should_Return_CategoryId()
    {
        var id = Guid.NewGuid();
        var categoryId = CategoryId.Create(id);
        categoryId.Value.ShouldBe(id);
    }

    [Fact(DisplayName = nameof(Create_Should_Throw_For_Empty_Guid))]
    public void Create_Should_Throw_For_Empty_Guid()
    {
        Should.Throw<DomainException>(() => CategoryId.Create(Guid.Empty));
    }
}