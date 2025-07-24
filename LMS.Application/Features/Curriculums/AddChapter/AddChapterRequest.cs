using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.Curriculums.AddChapter
{
    public sealed record AddChapterRequest(string Title, int SortOrder);
}
