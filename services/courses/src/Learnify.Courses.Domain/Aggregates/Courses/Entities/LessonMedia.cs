using Learnify.Courses.Domain.Aggregates.Courses.Enums;
using Learnify.Courses.Domain.Aggregates.Courses.ValueObjects;
using Learnify.Courses.Domain.SeedWork;

namespace Learnify.Courses.Domain.Aggregates.Courses.Entities;

public sealed class LessonMedia : Entity
{
    public MediaAssetId AssetId { get; private set; }
    public LessonMediaStatus Status { get; private set; }
    public TimeSpan? Duration { get; private set; }
    public string? FailureReason { get; private set; }

    #region EF Constructor

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private LessonMedia() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    #endregion

    private LessonMedia(MediaAssetId assetId)
    {
        AssetId = assetId;
        Status = LessonMediaStatus.PendingUpload;
    }

    public static LessonMedia Create(MediaAssetId assetId)
    {
        var lessonMedia = new LessonMedia(assetId);
        return lessonMedia;
    }

    public void MarkProcessing() => Status = LessonMediaStatus.Processing;

    public void MarkReady(TimeSpan? duration = null)
    {
        Status = LessonMediaStatus.Ready;
        Duration = duration;
        FailureReason = null;
    }

    public void MarkFailed(string? reason = null)
    {
        Status = LessonMediaStatus.Failed;
        FailureReason = reason;
    }
}