using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.LessonContents.AddImageWithCaption
{
    public sealed record AddImageWithCaptionRequest(int SortOrder, string? Caption, IFormFile ImageFile);
}
