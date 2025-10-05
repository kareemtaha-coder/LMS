using LMS.Application.Abstractions.Messaging;
using LMS.Application.Features.Shared;
using LMS.Domain.Abstractions;
using LMS.Domain.Lessons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.Lessons.GetAllLessonsInChapter
{
    public sealed record GetAllLessonsInChapterQuery(Guid ChapterId, LessonStatus? Status)
     : IQuery<Result<IReadOnlyList<LessonSummaryResponse>>>;
}
