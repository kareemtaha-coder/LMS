using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.LessonContents.AddExamplesGrid.ExampleItems.AddExampleItem
{
    public sealed record AddExampleItemRequest(IFormFile ImageFile, IFormFile? AudioFile);
}
