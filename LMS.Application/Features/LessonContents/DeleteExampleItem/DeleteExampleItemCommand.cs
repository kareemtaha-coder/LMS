using LMS.Application.Abstractions.Messaging;
using LMS.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.LessonContents.DeleteExampleItem
{
    public sealed record DeleteExampleItemCommand(Guid ItemId) : ICommand<Result>;
}
