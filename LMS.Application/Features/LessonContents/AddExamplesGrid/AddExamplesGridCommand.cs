using LMS.Application.Abstractions.Messaging;
using LMS.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.LessonContents.AddExamplesGrid
{
    public sealed record AddExamplesGridCommand(
     Guid LessonId,
     int SortOrder) : ICommand<Result<Guid>>;
}
