using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.Curriculums.GetCurriculum
{
    public sealed record CurriculumResponse(
    Guid Id,
    string Title,
    string Introduction,
    IReadOnlyCollection<ChapterResponse> Chapters);

    public sealed record ChapterResponse(
        Guid Id,
        string Title,
        int SortOrder,
        IReadOnlyCollection<LessonResponse> Lessons);

    public sealed record LessonResponse(
        Guid Id,
        string Title,
        int SortOrder);
}
