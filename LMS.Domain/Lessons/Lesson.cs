using LMS.Domain.Abstractions;
using LMS.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.Lessons
{
    public sealed class Lesson : Entity
    {
        private readonly List<LessonContent> _contents = new();

        public Title Title { get; private set; }
        public Guid ChapterId { get; private set; }
        public SortOrder SortOrder { get; private set; }
        public IReadOnlyCollection<LessonContent> Contents => _contents.AsReadOnly();

        private Lesson(Guid id, Title title, SortOrder sortOrder, Guid chapterId) : base(id)
        {
            Title = title;
            SortOrder = sortOrder;
            ChapterId = chapterId;
        }

        // Assumes the Create method on Lesson itself returns a Result<Lesson>
        public static Result<Lesson> Create(Title title, SortOrder sortOrder, Guid chapterId)
        {
            if (chapterId == Guid.Empty)
            {
                return Result.Failure<Lesson>(new Error("Lesson.NoChapterId", "Chapter ID cannot be empty."));
            }

            return new Lesson(Guid.NewGuid(), title, sortOrder, chapterId);
        }

        private Lesson() { } // For EF Core

        private Result CheckDuplicateSortOrder(SortOrder sortOrder)
        {
            return _contents.Any(c => c.SortOrder.Value == sortOrder.Value)
                ? Result.Failure(LessonErrors.DuplicateSortOrder)
                : Result.Success();
        }

        public Result AddRichTextContent(SortOrder sortOrder, string? arabicText, string? englishText)
        {
            var sortOrderCheck = CheckDuplicateSortOrder(sortOrder);
            if (sortOrderCheck.IsFailure) return sortOrderCheck;

            var contentResult = RichTextContent.Create(this.Id, sortOrder, arabicText, englishText);
            if (contentResult.IsFailure) return contentResult;

            _contents.Add(contentResult.Value);
            return Result.Success();
        }

        public Result AddVideoContent(SortOrder sortOrder, string videoUrl)
        {
            var sortOrderCheck = CheckDuplicateSortOrder(sortOrder);
            if (sortOrderCheck.IsFailure) return sortOrderCheck;

            var contentResult = VideoContent.Create(this.Id, sortOrder, videoUrl);
            if (contentResult.IsFailure) return contentResult;

            _contents.Add(contentResult.Value);
            return Result.Success();
        }

        public Result AddImageWithCaptionContent(SortOrder sortOrder, string imageUrl, string? caption)
        {
            var sortOrderCheck = CheckDuplicateSortOrder(sortOrder);
            if (sortOrderCheck.IsFailure) return sortOrderCheck;

            var contentResult = ImageWithCaptionContent.Create(this.Id, sortOrder, imageUrl, caption);
            if (contentResult.IsFailure) return contentResult;

            _contents.Add(contentResult.Value);
            return Result.Success();
        }

        public Result AddExamplesGridContent(SortOrder sortOrder)
        {
            var sortOrderCheck = CheckDuplicateSortOrder(sortOrder);
            if (sortOrderCheck.IsFailure) return sortOrderCheck;

            var contentResult = ExamplesGridContent.Create(this.Id, sortOrder);

            _contents.Add(contentResult.Value);
            return Result.Success();
        }

        public Result AddItemToExamplesGrid(Guid contentId, string imageUrl, string? audioUrl)
        {
            var content = _contents.FirstOrDefault(c => c.Id == contentId);
            if (content is null)
            {
                return Result.Failure(LessonErrors.ContentNotFound);
            }

            if (content is not ExamplesGridContent grid)
            {
                return Result.Failure(LessonErrors.InvalidContentType);
            }

            return grid.AddExampleItem(imageUrl, audioUrl);
        }
    }
}
