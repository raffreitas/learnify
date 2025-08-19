using Learnify.Courses.Domain.Aggregates.Instructors.ValueObjects;
using Learnify.Courses.Domain.SeedWork;

namespace Learnify.Courses.Domain.Aggregates.Instructors;

public sealed class Instructor : AggregateRoot
{
    public Name Name { get; private set; }
    public string Bio { get; private set; }
    public string ImageUrl { get; private set; }

    #region EF Constructor

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private Instructor() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    #endregion

    private Instructor(Name name, string bio, string imageUrl)
    {
        Name = name;
        Bio = bio;
        ImageUrl = imageUrl;
    }

    public static Instructor Create(Name name, string bio, string imageUrl)
    {
        return new Instructor(name, bio, imageUrl);
    }
}