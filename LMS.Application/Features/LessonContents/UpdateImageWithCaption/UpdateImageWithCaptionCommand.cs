using LMS.Application.Abstractions.Messaging;
using LMS.Domain.Abstractions;
using LMS.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.LessonContents.UpdateImageWithCaption
{
    public sealed record UpdateImageWithCaptionCommand(
    Guid ContentId,
    string? NewImageUrl, // Null if the image is not being replaced
    string? Caption, Title title) : ICommand<Result>;
}
