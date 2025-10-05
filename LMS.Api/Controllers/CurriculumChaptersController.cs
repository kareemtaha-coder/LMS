using LMS.Application.Features.Curriculums.AddChapter;
using LMS.Application.Features.Curriculums.Chapters.GetChapters;
using LMS.Domain.Curriculums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Api.Controllers
{
    [Route("api/curriculums/{curriculumId:guid}/chapters")]
    public sealed class CurriculumChaptersController : ApiControllerBase
    {
        public CurriculumChaptersController(ISender sender) : base(sender)
        {
        }

        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddChapter(
            Guid curriculumId,
            [FromBody] AddChapterRequest request,
            CancellationToken cancellationToken)
        {
            var command = new AddChapterCommand(
                curriculumId,
                request.Title,
                request.SortOrder);

            var result = await Sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return HandleFailure(result);
            }

            // For now, we return Ok. A fully RESTful API would return CreatedAtAction
            // with a route to a "GetChapterById" endpoint.
            return Ok(result.Value);
        }




        [HttpGet] // Route will be GET api/curriculums/{curriculumId}/chapters
        public async Task<IActionResult> GetChapters(
    [FromRoute] Guid curriculumId,
    CancellationToken cancellationToken)
        {
            // 1. Create the query object from the route parameter.
            var query = new GetChaptersQuery(curriculumId);

            // 2. Send the query to MediatR.
            var result = await Sender.Send(query, cancellationToken);

            // 3. Handle the result.
            if (result.IsFailure)
            {
                // This will handle the case where the curriculum itself is not found.
                return NotFound(result.Error);
            }

            // On success, return 200 OK with the list of chapters.
            return Ok(result.Value);
        }
    }
}