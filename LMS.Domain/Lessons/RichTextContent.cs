using LMS.Domain.Abstractions;
using LMS.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.Lessons
{
    public sealed class RichTextContent : LessonContent
    {
        public string? ArabicText { get; private set; }
        public string? EnglishText { get; private set; }

        private RichTextContent(Guid id, Guid lessonId, SortOrder sortOrder, string? arabicText, string? englishText)
            : base(id, lessonId, sortOrder)
        {
            ArabicText = arabicText;
            EnglishText = englishText;
        }

        internal static Result<RichTextContent> Create(Guid lessonId, SortOrder sortOrder, string? arabicText, string? englishText)
        {
            if (string.IsNullOrWhiteSpace(arabicText) && string.IsNullOrWhiteSpace(englishText))
            {
                return Result.Failure<RichTextContent>(LessonErrors.EmptyText);
            }

            var content = new RichTextContent(Guid.NewGuid(), lessonId, sortOrder, arabicText, englishText);
            return content;
        }

        private RichTextContent() { } // For EF Core
    }
}
