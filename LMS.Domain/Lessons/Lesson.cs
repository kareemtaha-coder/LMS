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
        private Lesson():base(Guid.NewGuid()) { }
        public static Lesson Create(Title title, SortOrder sortOrder, Guid chapterId)
        {
            if (chapterId == Guid.Empty)
            {
                throw new ArgumentException("Chapter ID cannot be empty.", nameof(chapterId));
            }

            return new Lesson(Guid.NewGuid(), title, sortOrder, chapterId);
        }

        // --- دوال إضافة المحتوى ---

        public void AddRichTextContent(SortOrder sortOrder, string? arabicText, string? englishText)
        {
            var content = RichTextContent.Create(this.Id, sortOrder, arabicText, englishText);
            _contents.Add(content);
        }

        public void AddVideoContent(SortOrder sortOrder, string videoUrl)
        {
            var content = VideoContent.Create(this.Id, sortOrder, videoUrl);
            _contents.Add(content);
        }

        public void AddImageWithCaptionContent(SortOrder sortOrder, string imageUrl, string? caption)
        {
            var content = ImageWithCaptionContent.Create(this.Id, sortOrder, imageUrl, caption);
            _contents.Add(content);
        }

        public ExamplesGridContent AddExamplesGridContent(SortOrder sortOrder)
        {
            if (_contents.Any(c => c.SortOrder.Value == sortOrder.Value))
            {
                throw new InvalidOperationException($"A content item with sort order {sortOrder.Value} already exists in this lesson.");
            }
            var content = ExamplesGridContent.Create(this.Id, sortOrder);
            _contents.Add(content);

            // نعيد الكائن للسماح بإضافة الأمثلة عليه مباشرةً
            return content;
        }
    }
}
