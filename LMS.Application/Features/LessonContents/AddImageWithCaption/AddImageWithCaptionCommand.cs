using LMS.Application.Abstractions.Messaging;
using LMS.Domain.Abstractions;
using LMS.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.LessonContents.AddImageWithCaption
{

    public sealed record AddImageWithCaptionCommand(
        Guid LessonId,
        string Title,
        int SortOrder,
        string ImageUrl,
        string? Caption) : ICommand<Result<Guid>>;
}
