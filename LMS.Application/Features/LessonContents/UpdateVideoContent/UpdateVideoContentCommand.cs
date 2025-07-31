using LMS.Application.Abstractions.Messaging;
using LMS.Domain.Abstractions;
using LMS.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.LessonContents.UpdateVideoContent
{
    public sealed record UpdateVideoContentCommand(
    Guid ContentId,
    string VideoUrl, string Title) : ICommand<Result>;
}
