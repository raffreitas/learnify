using System.Text.RegularExpressions;

namespace Learnify.Courses.Domain.Aggregates.Courses.ValueObjects;

public readonly partial record struct Slug
{
    public string Value { get; private init; }

    private Slug(string value)
    {
        Value = value;
    }

    public static Slug Create(string value, string? prefix = "", string suffix = "")
    {
        var slug = Generate(value, prefix, suffix);
        return new Slug(slug);
    }

    public static string Generate(string value, string? prefix = "", string suffix = "")
    {
        var slug = value.ToLowerInvariant();
        slug = ReplaceSpacesRegex().Replace(slug, "-");
        slug = RemoveInvalidCharacters().Replace(slug, "");

        slug = slug.Trim('-');

        if (!string.IsNullOrWhiteSpace(prefix))
            slug = $"{prefix}-{slug}";

        if (!string.IsNullOrWhiteSpace(suffix))
            slug = $"{slug}-{suffix}";

        return slug;
    }

    [GeneratedRegex(@"\s+")]
    private static partial Regex ReplaceSpacesRegex();

    [GeneratedRegex(@"[^a-z0-9\-]")]
    private static partial Regex RemoveInvalidCharacters();
}