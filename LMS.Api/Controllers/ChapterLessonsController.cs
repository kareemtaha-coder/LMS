using LMS.Application.Features.Curriculums.Chapters.GetChapterById;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChapterLessonsController : ApiControllerBase
    {
        private readonly ISender _sender;

        public ChapterLessonsController(ISender sender):base(sender)
        {
            _sender = sender;
        }

        [HttpGet("{chapterId:guid}")]
        public async Task<IActionResult> GetChapterById(
            Guid chapterId,
            CancellationToken cancellationToken)
        {
            var query = new GetChapterByIdQuery(chapterId);

            var result = await _sender.Send(query, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }
    }
}
