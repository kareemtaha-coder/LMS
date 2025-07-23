using LMS.Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Curriculums.CreateCurriculum
{
    public sealed record CreateCurriculumCommand(string Title,string Introduction) : ICommand<Guid>;
}

