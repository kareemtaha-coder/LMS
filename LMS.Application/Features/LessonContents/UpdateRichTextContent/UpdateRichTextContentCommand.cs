using LMS.Application.Abstractions.Messaging;
using LMS.Domain.Abstractions;
using LMS.Domain.Lessons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.LessonContents.UpdateRichTextContent
{
    public sealed record UpdateRichTextContentCommand(
    Guid ContentId,
    string? ArabicText,
    string? EnglishText,
    NoteType NoteType) : ICommand<Result>;
}
