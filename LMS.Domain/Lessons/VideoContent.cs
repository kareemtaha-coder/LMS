using LMS.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.Lessons
{
    /// <summary>
    /// يمثل قطعة محتوى من نوع "فيديو".
    /// </summary>
    public sealed class VideoContent : LessonContent
    {
        /// <summary>
        /// رابط الفيديو (على سبيل المثال، من يوتيوب أو فيميو).
        /// </summary>
        public string VideoUrl { get; private set; }

        /// <summary>
        /// Constructor خاص لضمان الإنشاء من خلال الـ Factory Method.
        /// </summary>
        private VideoContent(Guid id, Guid lessonId, SortOrder sortOrder, string videoUrl)
            : base(id, lessonId, sortOrder)
        {
            VideoUrl = videoUrl;
        }
        private VideoContent() : base(Guid.NewGuid(), Guid.Empty, new SortOrder(1)) { }
        /// <summary>
        /// دالة الإنشاء (Factory Method) لإنشاء كائن VideoContent جديد.
        /// </summary>
        /// <param name="lessonId">معرّف الدرس الأصل.</param>
        /// <param name="sortOrder">ترتيب المحتوى.</param>
        /// <param name="videoUrl">رابط الفيديو.</param>
        /// <returns>كائن VideoContent جديد وصالح.</returns>
        public static VideoContent Create(Guid lessonId, SortOrder sortOrder, string videoUrl)
        {
            // حماية الثوابت (Invariants)
            if (string.IsNullOrWhiteSpace(videoUrl))
            {
                throw new ArgumentException("Video URL cannot be empty.", nameof(videoUrl));
            }

            // يمكنك إضافة تحقق هنا للتأكد من أن الرابط هو URL صالح
            // if (!Uri.TryCreate(videoUrl, UriKind.Absolute, out _))
            // {
            //    throw new ArgumentException("Video URL is not a valid URL.", nameof(videoUrl));
            // }

            var videoContent = new VideoContent(Guid.NewGuid(), lessonId, sortOrder, videoUrl);

            return videoContent;
        }
    }
}