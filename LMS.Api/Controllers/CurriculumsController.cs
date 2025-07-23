using LMS.Application.Curriculums.CreateCurriculum;
using LMS.Domain.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurriculumsController : ControllerBase
    {
        private readonly ISender _sender;

        public CurriculumsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCurriculum(
            [FromBody] CreateCurriculumRequest request)
        {
            var command = new CreateCurriculumCommand(request.Title, request.Introduction);

            Result<Guid> result = await _sender.Send(command);

            if (result.IsFailure)
            {
                // التعامل مع أخطاء الـ validation يبقى كما هو لأنه صحيح
                if (result.Error is ValidationError validationError)
                {
                    var details = new ValidationProblemDetails(
                        validationError.Errors.ToDictionary(e => e.Code, e => new[] { e.Description })
                    );
                    return new BadRequestObjectResult(details);
                }

                // --- الجزء الذي تم تصحيحه ---
                // 1. إنشاء كائن ProblemDetails يدوياً
                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Bad Request",
                    Detail = result.Error.Description
                };

                // 2. إضافة الأخطاء المخصصة إلى خاصية Extensions
                problemDetails.Extensions.Add("errors", new[] { result.Error });

                // 3. إرجاع الكائن باستخدام ObjectResult مع تحديد الـ StatusCode
                return new ObjectResult(problemDetails)
                {
                    StatusCode = problemDetails.Status
                };
            }

            return CreatedAtAction(
                nameof(GetCurriculum),
                new { id = result.Value },
                result.Value);
        }

        [HttpGet("{id:guid}", Name = "GetCurriculum")]
        public IActionResult GetCurriculum(Guid id)
        {
            return Ok($"Get curriculum with ID: {id}");
        }
    }
}
