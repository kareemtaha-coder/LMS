using LMS.Domain.Abstractions;
using LMS.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.Lessons
{
    public abstract class LessonContent : Entity
    {
        public Guid LessonId { get; protected set; }
        public SortOrder SortOrder { get; protected set; }
        public Title Title { get; protected set; }

        protected LessonContent(Guid id, Guid lessonId, SortOrder sortOrder, Title title)
            : base(id)
        {
            LessonId = lessonId;
            SortOrder = sortOrder;
            Title = title;
        }

        protected LessonContent() { }

        internal void UpdateSortOrder(int newOrder)
        {
            var sortOrderResult = SortOrder.Create(newOrder);
            if (sortOrderResult.IsSuccess)
            {
                SortOrder = sortOrderResult.Value;
            }
        }

    }
}
