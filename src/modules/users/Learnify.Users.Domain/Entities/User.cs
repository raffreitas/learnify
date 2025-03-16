using Learnify.Core;
using Learnify.Users.Domain.Entities.ValueObjects;
using Learnify.Users.Domain.Enums;

namespace Learnify.Users.Domain.Entities;

public class User : Entity, IAggregateRoot
{
    public Name Name { get; private set; }
    public Email Email { get; private set; }
    public string Password { get; private set; }
    public DateOnly BirthDate { get; private set; }
    public string Document { get; private set; }
    public string Phone { get; private set; }
    public EUserRole Role { get; private set; }
    public bool IsActive { get; private set; }

    private User(Name name, Email email, string password, DateOnly birthDate, string document, string phone, EUserRole role)
    {
        Name = name;
        Email = email;
        Password = password;
        BirthDate = birthDate;
        Document = document;
        Phone = phone;
        Role = role;
        IsActive = true;
    }

    public static User Create(Name name, Email email, string password, DateOnly birthDate, string document, string phone, EUserRole role)
    {
        var user = new User(name, email, password, birthDate, document, phone, role);
        user.AddDomainEvent(new UserCreatedDomainEvent(user.Id, user.Email.Value));
        return user;
    }
}
