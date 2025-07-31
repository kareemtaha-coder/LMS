using LMS.Application.Features.Lessons.DeleteLesson;
using LMS.Application.Features.Lessons.GetLessonById;
using LMS.Application.Features.Lessons.PublishLesson;
using LMS.Application.Features.Lessons.UnpublishLesson;
using LMS.Application.Features.Lessons.UpdateLessonTitle;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonsController : ApiControllerBase
    {
        private readonly ISender _sender;

        public LessonsController(ISender sender) : base(sender)
        {
            _sender = sender;
        }

        [HttpGet("{lessonId:guid}")]
        [ProducesResponseType(typeof(LessonResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetLessonById(Guid lessonId, CancellationToken cancellationToken)
        {
            var query = new GetLessonByIdQuery(lessonId);

            var result = await _sender.Send(query, cancellationToken);

            if (result.IsFailure)
            {
                return HandleFailure(result);
            }

            return Ok(result.Value);
        }
        [HttpDelete("{lessonId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteLesson(
        Guid lessonId,
        CancellationToken cancellationToken)
        {
            var command = new DeleteLessonCommand(lessonId);
            var result = await Sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return HandleFailure(result);
            }

            return NoContent();
        }

        [HttpPut("{lessonId:guid}/publish")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PublishLesson(
        Guid lessonId,
        CancellationToken cancellationToken)
        {
            var command = new PublishLessonCommand(lessonId);
            var result = await Sender.Send(command, cancellationToken);
            if (result.IsFailure) return HandleFailure(result);
            return NoContent();
        }

        [HttpPut("{lessonId:guid}/unpublish")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UnpublishLesson(
            Guid lessonId,
            CancellationToken cancellationToken)
        {
            var command = new UnpublishLessonCommand(lessonId);
            var result = await Sender.Send(command, cancellationToken);
            if (result.IsFailure) return HandleFailure(result);
            return NoContent();
        }


        [HttpPut("{lessonId:guid}/title")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateLessonTitle(
            Guid lessonId,
            [FromBody] UpdateLessonTitleRequest request,
            CancellationToken cancellationToken)
        {
            var command = new UpdateLessonTitleCommand(lessonId, request.Title);
            var result = await Sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return HandleFailure(result);
            }

            return NoContent();
        }
    }
}
