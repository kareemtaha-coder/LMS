﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.Curriculums.UpdateCurriculum
{
    public sealed record UpdateCurriculumRequest(string Title, string Introduction);
}
