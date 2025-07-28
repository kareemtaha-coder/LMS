using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.Lessons.AddLessonToChapter
{
    public sealed record AddLessonToChapterRequest(string Title, int SortOrder);
}
