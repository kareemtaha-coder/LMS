using LMS.Domain.Abstractions;
using LMS.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.Lessons
{
    //--- 1. الكيان الابن: ExampleItem ---

    /// <summary>
    /// يمثل مثالاً واحدًا داخل شبكة الأمثلة، يحتوي على صورة وصوت اختياري.
    /// </summary>
    public sealed class ExampleItem : Entity
    {
        public Guid ExamplesGridContentId { get; private set; }
        public string ImageUrl { get; private set; }
        public string? AudioUrl { get; private set; }

        private ExampleItem(Guid id, Guid examplesGridContentId, string imageUrl, string? audioUrl) : base(id)
        {
            ExamplesGridContentId = examplesGridContentId;
            ImageUrl = imageUrl;
            AudioUrl = audioUrl;
        }
        private ExampleItem() : base(Guid.NewGuid()) { }

        /// <summary>
        /// دالة الإنشاء داخلية (internal) لتضمن أن ExamplesGridContent هو الوحيد الذي يمكنه إنشاؤها.
        /// </summary>
        internal static ExampleItem Create(Guid parentId, string imageUrl, string? audioUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                throw new ArgumentException("Example item image URL cannot be empty.", nameof(imageUrl));
            }

            return new ExampleItem(Guid.NewGuid(), parentId, imageUrl, audioUrl);
        }
    }


    //--- 2. الكيان الأب: ExamplesGridContent ---

    /// <summary>
    /// يمثل قطعة محتوى من نوع "شبكة أمثلة".
    /// هو الـ Aggregate Root لكيانات ExampleItem.
    /// </summary>
    public sealed class ExamplesGridContent : LessonContent
    {
        private readonly List<ExampleItem> _exampleItems = new();
        public IReadOnlyCollection<ExampleItem> ExampleItems => _exampleItems.AsReadOnly();
        private ExamplesGridContent() : base(Guid.NewGuid(), Guid.Empty, new SortOrder(1)) { }
        private ExamplesGridContent(Guid id, Guid lessonId, SortOrder sortOrder)
            : base(id, lessonId, sortOrder)
        {
        }

        public static ExamplesGridContent Create(Guid lessonId, SortOrder sortOrder)
        {
            // هذا الكيان هو مجرد حاوية، لذا لا يحتاج لبارامترات إضافية عند إنشائه.
            return new ExamplesGridContent(Guid.NewGuid(), lessonId, sortOrder);
        }

        /// <summary>
        /// دالة لإضافة مثال جديد إلى الشبكة.
        /// </summary>
        public void AddExampleItem(string imageUrl, string? audioUrl)
        {
            // قاعدة عمل: لا يمكن أن تحتوي الشبكة على أكثر من 6 أمثلة
            if (_exampleItems.Count >= 6)
            {
                throw new InvalidOperationException("Cannot add more than 6 examples to a single grid.");
            }

            var exampleItem = ExampleItem.Create(Id, imageUrl, audioUrl);
            _exampleItems.Add(exampleItem);
        }
    }
}
