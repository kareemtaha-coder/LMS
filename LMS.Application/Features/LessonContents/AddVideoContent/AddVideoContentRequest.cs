using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.LessonContents.AddVideoContent
{
    public sealed record AddVideoContentRequest(int SortOrder, string VideoUrl);
}
