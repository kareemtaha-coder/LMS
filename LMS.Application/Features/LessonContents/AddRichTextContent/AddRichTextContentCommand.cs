using LMS.Application.Abstractions.Messaging;
using LMS.Domain.Abstractions;
using LMS.Domain.Lessons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.LessonContents.AddRichTextContent
{
    public sealed record AddRichTextContentCommand(
     Guid LessonId,
     int SortOrder,
     string? ArabicText,
     string? EnglishText,
     NoteType NoteType) : ICommand<Result<Guid>>;
}
