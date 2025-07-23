using LMS.Domain.Abstractions;
using LMS.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.Lessons
{
    /// <summary>
    /// يمثل الكلاس الأساسي التجريدي لكل أنواع المحتوى داخل الدرس.
    /// يحتوي على الخصائص المشتركة بين كل أنواع المحتوى.
    /// </summary>
    public abstract class LessonContent : Entity
    {
        /// <summary>
        /// معرّف الدرس الذي ينتمي إليه هذا المحتوى.
        /// </summary>
        public Guid LessonId { get; private set; }

        /// <summary>
        /// ترتيب ظهور هذا المحتوى داخل الدرس.
        /// </summary>
        public SortOrder SortOrder { get; private set; }

        /// <summary>
        /// الـ constructor محمي (protected) ليضمن أن الكلاسات الوارثة فقط هي التي يمكنها استدعاؤه.
        /// </summary>
        /// <param name="id">المعرّف الفريد لقطعة المحتوى.</param>
        /// <param name="lessonId">معرّف الدرس الأصل.</param>
        /// <param name="sortOrder">ترتيب المحتوى.</param>
        protected LessonContent(Guid id, Guid lessonId, SortOrder sortOrder)
            : base(id)
        {
            LessonId = lessonId;
            SortOrder = sortOrder;
        }
    }
}
