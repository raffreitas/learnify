using Learnify.Courses.Domain.Aggregates.Courses.ValueObjects;
using Learnify.Courses.Domain.Exceptions;

using Shouldly;

namespace Learnify.Courses.UnitTests.Domain.Aggregates.Courses.ValueObjects;

[Trait("UnitTests", "Domain - Aggregates")]
public sealed class PriceTests
{
    [Fact(DisplayName = nameof(Create_Should_Returns_Price))]
    public void Create_Should_Returns_Price()
    {
        // Arrange
        decimal value = 100.00m;
        string currency = "USD";

        // Act
        var price = Price.Create(value, currency);

        // Assert
        price.Value.ShouldBe(value);
        price.Currency.ShouldBe(currency);
    }

    [Fact(DisplayName = nameof(Create_Should_Returns_Price_With_Default_BRL_Currency))]
    public void Create_Should_Returns_Price_With_Default_BRL_Currency()
    {
        // Arrange
        decimal value = 100.00m;
        string expectedCurrency = "BRL";

        // Act
        var price = Price.Create(value);

        // Assert
        price.Value.ShouldBe(value);
        price.Currency.ShouldBe(expectedCurrency);
    }

    [Fact(DisplayName = nameof(Create_Should_Throws_When_Price_Is_Negative))]
    public void Create_Should_Throws_When_Price_Is_Negative()
    {
        // Arrange
        decimal value = -1;

        //Act & Assert
        Should.Throw<DomainException>(() => Price.Create(value))
            .Message.ShouldBe("value cannot be negative.");
    }
}
