using Learnify.VideoProcessing.Domain.SeedWork;

namespace Learnify.VideoProcessing.Domain.Aggregates.Videos;

public class Video : AggregateRoot
{
    public string Filename { get; private set; }
    public string? Url { get; private set; }
    public TimeSpan? Duration { get; private set; }

    private Video(string filename)
    {
        Filename = filename;
    }

    public static Video Create(string filename)
    {
        return new Video(filename);
    }
}