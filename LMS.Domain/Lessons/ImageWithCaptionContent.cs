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

        private ImageWithCaptionContent(Guid id, Guid lessonId, SortOrder sortOrder, string imageUrl, string? caption ,Title title )
            : base(id, lessonId, sortOrder ,title)
        {
            ImageUrl = imageUrl;
            Caption = caption;
            Title = title;
        }

        internal static Result<ImageWithCaptionContent> Create(Guid lessonId, SortOrder sortOrder, string imageUrl, string? caption, Title title)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                return Result.Failure<ImageWithCaptionContent>(LessonErrors.EmptyUrl(nameof(ImageWithCaptionContent)));
            }

            var content = new ImageWithCaptionContent(Guid.NewGuid(), lessonId, sortOrder, imageUrl, caption,title);
            return content;
        }

        internal Result Update(string newImageUrl, string? newCaption, Title newTitle)
        {
            if (string.IsNullOrWhiteSpace(newImageUrl))
            {
                return Result.Failure(LessonErrors.EmptyUrl(nameof(ImageWithCaptionContent)));
            }
            Title = newTitle;
            ImageUrl = newImageUrl;
            Caption = newCaption;
            return Result.Success();
        }
        private ImageWithCaptionContent() { } // For EF Core
    }

}
