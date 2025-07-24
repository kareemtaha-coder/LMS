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

        private VideoContent(Guid id, Guid lessonId, SortOrder sortOrder, string videoUrl)
            : base(id, lessonId, sortOrder)
        {
            VideoUrl = videoUrl;
        }

        internal static Result<VideoContent> Create(Guid lessonId, SortOrder sortOrder, string videoUrl)
        {
            if (string.IsNullOrWhiteSpace(videoUrl))
            {
                return Result.Failure<VideoContent>(LessonErrors.EmptyUrl(nameof(VideoContent)));
            }

            if (!Uri.TryCreate(videoUrl, UriKind.Absolute, out _))
            {
                return Result.Failure<VideoContent>(LessonErrors.InvalidUrl(nameof(VideoContent)));
            }

            var content = new VideoContent(Guid.NewGuid(), lessonId, sortOrder, videoUrl);
            return content;
        }

        private VideoContent() { } // For EF Core
    }
}
