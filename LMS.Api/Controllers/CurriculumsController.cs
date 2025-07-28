using LMS.Application.Curriculums.CreateCurriculum;
using LMS.Application.Features.Curriculums.DeleteCurriculum;
using LMS.Application.Features.Curriculums.GetAllCurriculums;
using LMS.Application.Features.Curriculums.GetCurriculum;
using LMS.Application.Features.Curriculums.UpdateCurriculum;
using LMS.Domain.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace LMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurriculumsController(ISender sender) : ApiControllerBase(sender)
    {
        private readonly ISender _sender = sender;

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetCurriculumById(Guid id, CancellationToken cancellationToken)
        {
            // 1. إنشاء الـ Query
            var query = new GetCurriculumQuery(id);

            // 2. إرسال الـ Query إلى MediatR
            var result = await _sender.Send(query, cancellationToken);

            // 3. ترجمة النتيجة إلى استجابة HTTP
            return result.IsSuccess
                ? Ok(result.Value)
                : NotFound(result.Error);
        }

            // This is the new endpoint
            [HttpGet]
            [ProducesResponseType(typeof(IReadOnlyList<CurriculumSummaryResponse>), StatusCodes.Status200OK)]
            public async Task<IActionResult> GetAllCurriculums(CancellationToken cancellationToken)
            {
                var query = new GetAllCurriculumsQuery();

                var result = await Sender.Send(query, cancellationToken);

                // For a query, a failure is unlikely but good to handle.
                // A success result with an empty list is a valid scenario.
                return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
            }

            [HttpPost]
            [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            public async Task<IActionResult> CreateCurriculum(
                [FromBody] CreateCurriculumCommand command,
                CancellationToken cancellationToken)
            {
                var result = await Sender.Send(command, cancellationToken);

                if (result.IsFailure)
                {
                    return HandleFailure(result);
                }

                return CreatedAtAction(
                    nameof(GetCurriculumById),
                    new { id = result.Value },
                    result.Value);
            }
        [HttpPut("{curriculumId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCurriculum(
        Guid curriculumId,
        [FromBody] UpdateCurriculumRequest request,
        CancellationToken cancellationToken)
        {
            var command = new UpdateCurriculumCommand(
                curriculumId,
                request.Title,
                request.Introduction);

            var result = await Sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return HandleFailure(result);
            }

            return NoContent();
        }

        [HttpDelete("{curriculumId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteCurriculum(
       Guid curriculumId,
       CancellationToken cancellationToken)
        {
            var command = new DeleteCurriculumCommand(curriculumId);
            var result = await Sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return HandleFailure(result);
            }

            return NoContent();
        }

    }
    }
