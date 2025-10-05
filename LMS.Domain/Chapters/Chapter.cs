using LMS.Domain.Abstractions;
using LMS.Domain.Curriculums;
using LMS.Domain.Lessons;
using LMS.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.Chapters
{
    public sealed class Chapter : Entity
    {
        private readonly List<Lesson> _lessons = new();

        public Title Title { get; private set; } = default!;
        public Guid CurriculumId { get; private set; }
        public SortOrder SortOrder { get; private set; } = default!;
        public IReadOnlyCollection<Lesson> Lessons => _lessons.AsReadOnly();

        private Chapter( Title title, Guid curriculumId, SortOrder sortOrder) 
        {
            Title = title;
            CurriculumId = curriculumId;
            SortOrder = sortOrder;
        }

        // Updated constructor to call base constructor explicitly
        private Chapter() { } // For EF Core

        public static Result<Chapter> Create(Title title, Guid curriculumId, SortOrder sortOrder)
        {
            if (curriculumId == Guid.Empty)
            {
                // We return a failure result, not an exception
                return Result.Failure<Chapter>(new Error(
                    "Chapter.MissingCurriculumId",
                    "The curriculum ID is required to create a chapter."));
            }

            var chapter = new Chapter(title, curriculumId, sortOrder);

            // The 'Result' class has an implicit conversion, so this is the same as 'Result.Success(chapter)'
            return chapter;
        }

        internal Result AddLesson(Title title, SortOrder sortOrder)
        {
            if (_lessons.Any(l => l.Title.Value == title.Value))
            {
                return Result.Failure(CurriculumErrors.DuplicateLessonTitle);
            }

            var lessonResult = Lesson.Create(title, sortOrder, this.Id);
            if (lessonResult.IsFailure)
            {
                return lessonResult;
            }

            _lessons.Add(lessonResult.Value);
            return Result.Success();
        }
        internal Result<IReadOnlyList<LessonContent>> RemoveLesson(Guid lessonId)
        {
            var lessonToRemove = _lessons.FirstOrDefault(l => l.Id == lessonId);
            if (lessonToRemove is null)
            {
                return Result.Failure<IReadOnlyList<LessonContent>>(CurriculumErrors.LessonNotFound);
            }

            var contentsToDelete = lessonToRemove.Contents.ToList();
            _lessons.Remove(lessonToRemove);

            return contentsToDelete;
        }
        internal Result UpdateTitle(Title newTitle)
        {
            // Add any specific validation for a chapter's title if needed.
            // For now, the Title value object's own validation is sufficient.
            Title = newTitle;
            return Result.Success();
        }
    }
}
