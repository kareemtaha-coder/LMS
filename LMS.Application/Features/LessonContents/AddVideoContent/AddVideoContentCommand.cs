using LMS.Application.Abstractions.Messaging;
using LMS.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.LessonContents.AddVideoContent
{
    public sealed record AddVideoContentCommand(
    Guid LessonId,
    int SortOrder,
    string VideoUrl) : ICommand<Result<Guid>>;
}
