using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.Shared
{
    public sealed record LessonSummaryResponse(
    Guid Id,
    string Title,
    int SortOrder);
}
