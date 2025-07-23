using LMS.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.Lessons
{
    /// <summary>
    /// يمثل قطعة محتوى من نوع "نص مزدوج" (عربي وإنجليزي).
    /// </summary>
    public sealed class RichTextContent : LessonContent
    {
        /// <summary>
        /// النص باللغة العربية. يمكن أن يكون فارغًا.
        /// </summary>
        public string? ArabicText { get; private set; }

        /// <summary>
        /// النص باللغة الإنجليزية. يمكن أن يكون فارغًا.
        /// </summary>
        public string? EnglishText { get; private set; }

        /// <summary>
        /// Constructor خاص لضمان الإنشاء من خلال الـ Factory Method.
        /// </summary>
        private RichTextContent(Guid id, Guid lessonId, SortOrder sortOrder, string? arabicText, string? englishText)
            : base(id, lessonId, sortOrder)
        {
            ArabicText = arabicText;
            EnglishText = englishText;
        }
        private RichTextContent() : base(Guid.NewGuid(), Guid.Empty, new SortOrder(1)) { }
        /// <summary>
        /// دالة الإنشاء (Factory Method) لإنشاء كائن RichTextContent جديد.
        /// </summary>
        /// <param name="lessonId">معرّف الدرس الأصل.</param>
        /// <param name="sortOrder">ترتيب المحتوى.</param>
        /// <param name="arabicText">النص بالعربية.</param>
        /// <param name="englishText">النص بالإنجليزية.</param>
        /// <returns>كائن RichTextContent جديد وصالح.</returns>
        public static RichTextContent Create(Guid lessonId, SortOrder sortOrder, string? arabicText, string? englishText)
        {
            // حماية الثوابت (Invariants):
            // يجب أن يحتوي على نص واحد على الأقل.
            if (string.IsNullOrWhiteSpace(arabicText) && string.IsNullOrWhiteSpace(englishText))
            {
                throw new ArgumentException("At least one text (Arabic or English) must be provided.");
            }

            var richTextContent = new RichTextContent(Guid.NewGuid(), lessonId, sortOrder, arabicText, englishText);

            return richTextContent;
        }
    }
}