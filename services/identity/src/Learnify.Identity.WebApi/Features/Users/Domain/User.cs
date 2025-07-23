using Learnify.Identity.WebApi.Features.Users.Domain.Enums;

namespace Learnify.Identity.WebApi.Features.Users.Domain;

public sealed class User
{
    public Guid Id { get; private set; }
    public string ProviderKey { get; private set; }
    public string ProviderType { get; private set; }
    public string Email { get; private set; }
    public string FirstName { get; private set; }
    public string? LastName { get; private set; }
    public UserRole Role { get; private set; }
    public string? Picture { get; private set; }
    public bool IsActive { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    private User(
        Guid id,
        string providerKey,
        string providerType,
        UserRole role,
        string email,
        string firstName,
        string? lastName = null,
        string? picture = null
    )
    {
        Id = id;
        ProviderKey = providerKey;
        ProviderType = providerType;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        Role = role;
        IsActive = true;
        Picture = picture;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public static User Create(
        string providerKey,
        string providerType,
        string email,
        UserRole role,
        string firstName,
        string? lastName = null,
        string? picture = null
    )
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(providerKey);
        ArgumentException.ThrowIfNullOrWhiteSpace(providerType);
        ArgumentException.ThrowIfNullOrWhiteSpace(firstName);

        return new User(Guid.NewGuid(), providerKey, providerType, role, email, firstName, lastName, picture);
    }

    public void Update(string firstName, string? lastName, string? picture)
    {
        FirstName = firstName;
        LastName = lastName;
        Picture = picture;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Deactivate()
    {
        if (IsActive)
        {
            IsActive = false;
            UpdatedAt = DateTimeOffset.UtcNow;
        }
    }

    public void Activate()
    {
        if (!IsActive)
        {
            IsActive = true;
            UpdatedAt = DateTimeOffset.UtcNow;
        }
    }
}
