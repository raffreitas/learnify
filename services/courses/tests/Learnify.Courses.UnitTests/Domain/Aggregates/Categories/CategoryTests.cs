using Learnify.Courses.Domain.Aggregates.Categories;
using Learnify.Courses.Domain.Exceptions;

using Shouldly;

namespace Learnify.Courses.UnitTests.Domain.Aggregates.Categories;

[Trait("UnitTests", "Domain - Aggregates")]
public sealed class CategoryTests
{
    [Fact(DisplayName = nameof(Create_Should_Return_Category))]
    public void Create_Should_Return_Category()
    {
        const string name = "Programming";
        var category = Category.Create(name);
        category.Name.ShouldBe(name);
    }

    [Theory(DisplayName = nameof(Create_Should_Throw_For_Invalid_Name))]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_Should_Throw_For_Invalid_Name(string invalid)
    {
        Should.Throw<DomainException>(() => Category.Create(invalid));
    }
}