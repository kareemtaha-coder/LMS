using LMS.Domain.Abstractions;
using LMS.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.Lessons
{
    public sealed class VideoContent : LessonContent
    {
        public string VideoUrl { get; private set; }

        private VideoContent(Guid id, Guid lessonId, SortOrder sortOrder, string videoUrl,Title title)
            : base(id, lessonId, sortOrder, title)
        {
            VideoUrl = videoUrl;
        }

        internal static Result<VideoContent> Create(Guid lessonId, SortOrder sortOrder, string videoUrl, Title title)
        {
            if (string.IsNullOrWhiteSpace(videoUrl))
            {
                return Result.Failure<VideoContent>(LessonErrors.EmptyUrl(nameof(VideoContent)));
            }

            if (!Uri.TryCreate(videoUrl, UriKind.Absolute, out _))
            {
                return Result.Failure<VideoContent>(LessonErrors.InvalidUrl(nameof(VideoContent)));
            }

            var content = new VideoContent(Guid.NewGuid(), lessonId, sortOrder, videoUrl, title);
            return content;
        }
        internal Result Update(string newVideoUrl)
        {
            if (string.IsNullOrWhiteSpace(newVideoUrl))
            {
                return Result.Failure(LessonErrors.EmptyUrl(nameof(VideoContent)));
            }

            if (!Uri.TryCreate(newVideoUrl, UriKind.Absolute, out _))
            {
                return Result.Failure(LessonErrors.InvalidUrl(nameof(VideoContent)));
            }

            VideoUrl = newVideoUrl;
            return Result.Success();
        }
        private VideoContent() { } // For EF Core
    }
}
