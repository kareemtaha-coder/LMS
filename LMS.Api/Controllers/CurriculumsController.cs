using LMS.Application.Curriculums;
using LMS.Application.Curriculums.CreateCurriculum;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurriculumsController : ControllerBase
    {
        private readonly ISender sender;

        public CurriculumsController(ISender sender)
        {
           this.sender = sender;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCurriculum(
            [FromBody] CreateCurriculumRequest request)
        {
            var command = new CreateCurriculumCommand(request.Title, request.Introduction);

            var curriculumId = await sender.Send(command);

            return CreatedAtAction(nameof(GetCurriculum), new { id = curriculumId }, curriculumId);
        }

        [HttpGet("{id:guid}")]
        public IActionResult GetCurriculum(Guid id)
        {
            return Ok();
        }
    }
}
