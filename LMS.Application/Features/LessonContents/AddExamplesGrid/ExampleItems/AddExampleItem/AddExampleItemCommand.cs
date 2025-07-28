using LMS.Application.Abstractions.Messaging;
using LMS.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.LessonContents.AddExamplesGrid.ExampleItems.AddExampleItem
{
    public sealed record AddExampleItemCommand(
    Guid ContentId, // The ID of the ExamplesGridContent
    string ImageUrl,
    string? AudioUrl) : ICommand<Result<Guid>>;
}
