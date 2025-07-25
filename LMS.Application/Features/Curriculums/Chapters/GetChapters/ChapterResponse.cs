using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.Curriculums.Chapters.GetChapters
{
    public sealed record ChapterResponse(
     Guid Id,
     string Title,
     int SortOrder);
}
