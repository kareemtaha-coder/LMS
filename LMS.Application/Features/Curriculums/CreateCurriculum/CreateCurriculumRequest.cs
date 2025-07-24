using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.Curriculums.CreateCurriculum
{
    public sealed record CreateCurriculumRequest(
       string Title,
       string Introduction);
}
