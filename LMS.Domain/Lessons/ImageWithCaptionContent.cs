using LMS.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.Lessons
{
    /// <summary>
    /// يمثل قطعة محتوى من نوع "صورة مع تعليق".
    /// </summary>
    public sealed class ImageWithCaptionContent : LessonContent
    {
        /// <summary>
        /// مسار الصورة التي تم رفعها.
        /// </summary>
        public string ImageUrl { get; private set; }

        /// <summary>
        /// التعليق النصي المصاحب للصورة. يمكن أن يكون فارغًا.
        /// </summary>
        public string? Caption { get; private set; }

        /// <summary>
        /// Constructor خاص لضمان الإنشاء من خلال الـ Factory Method.
        /// </summary>
        private ImageWithCaptionContent(Guid id, Guid lessonId, SortOrder sortOrder, string imageUrl, string? caption)
            : base(id, lessonId, sortOrder)
        {
            ImageUrl = imageUrl;
            Caption = caption;
        }
        private ImageWithCaptionContent() : base(Guid.NewGuid(), Guid.Empty, new SortOrder(1)) { }

        /// <summary>
        /// دالة الإنشاء (Factory Method) لإنشاء كائن ImageWithCaptionContent جديد.
        /// </summary>
        /// <param name="lessonId">معرّف الدرس الأصل.</param>
        /// <param name="sortOrder">ترتيب المحتوى.</param>
        /// <param name="imageUrl">مسار الصورة.</param>
        /// <param name="caption">التعليق المصاحب.</param>
        /// <returns>كائن ImageWithCaptionContent جديد وصالح.</returns>
        public static ImageWithCaptionContent Create(Guid lessonId, SortOrder sortOrder, string imageUrl, string? caption)
        {
            // حماية الثوابت (Invariants)
            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                throw new ArgumentException("Image URL cannot be empty.", nameof(imageUrl));
            }


            var imageContent = new ImageWithCaptionContent(Guid.NewGuid(), lessonId, sortOrder, imageUrl, caption);

            return imageContent;
        }
    }

}
