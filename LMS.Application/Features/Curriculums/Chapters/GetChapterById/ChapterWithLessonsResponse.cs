using LMS.Application.Features.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.Curriculums.Chapters.GetChapterById
{
    public sealed record ChapterWithLessonsResponse(
     Guid Id,
     string Title,
     int SortOrder,
     List<LessonSummaryResponse> Lessons);
}
