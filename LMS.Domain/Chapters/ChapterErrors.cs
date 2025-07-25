using LMS.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.Chapters
{
    public static class ChapterErrors
    {
        public static readonly Error NotFound = new(
            "Chapter.NotFound",
            "The chapter with the specified identifier was not found.");
    }
}
