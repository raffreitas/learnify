using OpenLibs.SeedWork;

namespace Learnify.VideoProcessing.Domain.Aggregates.Videos;

public class Video : AggregateRoot
{
    public string Title { get; private set; }
    public string Url { get; private set; }
    public TimeSpan Duration { get; private set; }

    private Video(Guid id, string title, string url, TimeSpan duration)
    {
        Id = id;
        Title = title;
        Url = url;
        Duration = duration;
    }

    public static Video Create(string title, string url, TimeSpan duration)
    {
        return new Video(Guid.NewGuid(), title, url, duration);
    }
}