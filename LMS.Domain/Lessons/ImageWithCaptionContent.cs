using LMS.Domain.Abstractions;
using LMS.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.Lessons
{
    public sealed class ImageWithCaptionContent : LessonContent
    {
        public string ImageUrl { get; private set; }
        public string? Caption { get; private set; }

        private ImageWithCaptionContent(Guid id, Guid lessonId, SortOrder sortOrder, string imageUrl, string? caption)
            : base(id, lessonId, sortOrder)
        {
            ImageUrl = imageUrl;
            Caption = caption;
        }

        internal static Result<ImageWithCaptionContent> Create(Guid lessonId, SortOrder sortOrder, string imageUrl, string? caption)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                return Result.Failure<ImageWithCaptionContent>(LessonErrors.EmptyUrl(nameof(ImageWithCaptionContent)));
            }

            var content = new ImageWithCaptionContent(Guid.NewGuid(), lessonId, sortOrder, imageUrl, caption);
            return content;
        }

        private ImageWithCaptionContent() { } // For EF Core
    }

}
