using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.LessonContents.UpdateImageWithCaption
{
    public sealed record UpdateImageWithCaptionRequest(string? Caption, IFormFile? ImageFile);
}
