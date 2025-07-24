using LMS.Application.Abstractions.Messaging;
using LMS.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.Curriculums.GetAllCurriculums
{
    public sealed record GetAllCurriculumsQuery()
    : IQuery<Result<IReadOnlyCollection<CurriculumSummaryResponse>>>;
}
