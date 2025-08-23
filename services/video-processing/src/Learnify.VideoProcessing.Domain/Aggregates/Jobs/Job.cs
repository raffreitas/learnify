using OpenLibs.SeedWork;

namespace Learnify.VideoProcessing.Domain.Aggregates.Jobs;

public class Job : AggregateRoot
{
    public Status Status { get; private set; }
}

public enum Status
{
    Downloading,
    Fragmenting,
    Encoding,
    Finishing,
    Completed,
    Failed
}