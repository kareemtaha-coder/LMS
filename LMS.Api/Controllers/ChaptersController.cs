using LMS.Application.Features.Curriculums.Chapters.DeleteChapter;
using LMS.Application.Features.Curriculums.Chapters.UpdateChapter;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Api.Controllers
{

    [Route("api/chapters")]
    [ApiController]
    public class ChaptersController : ApiControllerBase
    {
        public ChaptersController(ISender sender) : base(sender)
        {
        }

        [HttpPut("{chapterId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateChapter(
            Guid chapterId,
            [FromBody] UpdateChapterRequest request,
            CancellationToken cancellationToken)
        {
            var command = new UpdateChapterCommand(
                chapterId,
                request.Title);

            var result = await Sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return HandleFailure(result);
            }

            return NoContent();
        }

        [HttpDelete("{chapterId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteChapter(
        Guid chapterId,
        CancellationToken cancellationToken)
        {
            var command = new DeleteChapterCommand(chapterId);
            var result = await Sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return HandleFailure(result);
            }

            return NoContent();
        }
    }
}
