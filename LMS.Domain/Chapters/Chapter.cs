using LMS.Domain.Abstractions;
using LMS.Domain.Lessons;
using LMS.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.Chapters
{
    public class Chapter:Entity
    {
        private Chapter(Guid id, Title title, Guid curriculumId, SortOrder sortOrder) : base(id)
        {
            Title = title;
            CurriculumId = curriculumId;
            SortOrder = sortOrder;
        }
        private Chapter() : base(Guid.NewGuid()) { }

        public Title Title { get; private set; }
        public Guid CurriculumId { get; private set; }
        public SortOrder SortOrder { get;private set; }
        private readonly List<Lesson> lessons = new();
        public IReadOnlyCollection<Lesson> Lessons => lessons.AsReadOnly();

        public static Chapter Create(Title title, Guid curriculumId, SortOrder sortOrder)
        {
            if (curriculumId == Guid.Empty)
            {
                throw new ArgumentException("Curriculum ID cannot be empty.", nameof(curriculumId));
            }
            var chapter = new Chapter(Guid.NewGuid(), title, curriculumId, sortOrder);
            return chapter;
        }

        public void AddLesson(Title title, SortOrder sortOrder)
        {
            // قاعدة عمل اختيارية: منع تكرار الدروس بنفس العنوان داخل نفس الفصل
            if (lessons.Any(l => l.Title.Value == title.Value))
            {
                throw new InvalidOperationException("A lesson with this title already exists in this chapter.");
            }

            // إنشاء الدرس الجديد من خلال الـ Factory الخاص به
            var lesson = Lesson.Create(title, sortOrder, this.Id);

            // إضافة الدرس للقائمة الداخلية
            lessons.Add(lesson);
        }

    }
}
