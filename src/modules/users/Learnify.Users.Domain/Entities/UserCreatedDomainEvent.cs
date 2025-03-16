using Learnify.Core;

namespace Learnify.Users.Domain.Entities;

public record UserCreatedDomainEvent(Guid UserId, string Email) : DomainEvent;
