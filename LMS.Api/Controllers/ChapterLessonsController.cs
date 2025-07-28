using LMS.Application.Features.Curriculums.Chapters.GetChapterById;
using LMS.Application.Features.Lessons;
using LMS.Application.Features.Lessons.AddLessonToChapter;
using LMS.Application.Features.Lessons.GetAllLessonsInChapter;
using LMS.Application.Features.Shared;
using LMS.Domain.Lessons;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Api.Controllers
{
    [Route("api/Chapters")]
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


        [HttpPost("{chapterId:guid}/lessons")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddLessonToChapter(
        Guid chapterId,
        [FromBody] AddLessonToChapterRequest request,
        CancellationToken cancellationToken)
        {
            var command = new AddLessonToChapterCommand(
                chapterId,
                request.Title,
                request.SortOrder);

            var result = await Sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return HandleFailure(result);
            }

            return Ok(result.Value);
        }

        [HttpGet("{chapterId:guid}/lessons")]
        [ProducesResponseType(typeof(IReadOnlyList<LessonSummaryResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllLessonsInChapter(
       Guid chapterId,
       [FromQuery] LessonStatus? status, // Add this optional parameter
       CancellationToken cancellationToken)
        {
            // Pass the status to the query
            var query = new GetAllLessonsInChapterQuery(chapterId, status);

            var result = await Sender.Send(query, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
        }
    }

}
