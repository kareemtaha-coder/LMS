using LMS.Application.Features.LessonContents.AddExamplesGrid.ExampleItems.AddExampleItem;
using LMS.Application.Features.LessonContents.DeleteExampleItem;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Api.Controllers
{
    [Route("api/contents/{contentId:guid}/example-items")]
    [ApiController]
    public class ExampleItemsController : ApiControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ExampleItemsController(ISender sender, IWebHostEnvironment webHostEnvironment) : base(sender)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddExampleItem(
            Guid contentId,
            [FromForm] AddExampleItemRequest request,
            CancellationToken cancellationToken)
        {
            if (request.ImageFile is null || request.ImageFile.Length == 0)
            {
                return BadRequest("Image file is required.");
            }

            // --- Save Image File ---
            var imageUrl = await SaveFileAsync(request.ImageFile, "images/examples", cancellationToken);

            // --- Save Optional Audio File ---
            string? audioUrl = null;
            if (request.AudioFile is not null && request.AudioFile.Length > 0)
            {
                audioUrl = await SaveFileAsync(request.AudioFile, "audio/examples", cancellationToken);
            }

            // --- Send Command ---
            var command = new AddExampleItemCommand(contentId, imageUrl, audioUrl);
            var result = await Sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                // Clean up uploaded files if command fails
                if (System.IO.File.Exists(Path.Combine(_webHostEnvironment.WebRootPath, imageUrl.TrimStart('/'))))
                    System.IO.File.Delete(Path.Combine(_webHostEnvironment.WebRootPath, imageUrl.TrimStart('/')));
                if (audioUrl is not null && System.IO.File.Exists(Path.Combine(_webHostEnvironment.WebRootPath, audioUrl.TrimStart('/'))))
                    System.IO.File.Delete(Path.Combine(_webHostEnvironment.WebRootPath, audioUrl.TrimStart('/')));

                return HandleFailure(result);
            }

            return Ok(result.Value);
        }

        private async Task<string> SaveFileAsync(IFormFile file, string subfolder, CancellationToken cancellationToken)
        {
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, subfolder);
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            await using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream, cancellationToken);
            }

            return $"/{subfolder}/{uniqueFileName}";
        }

       
            [HttpDelete("{itemId:guid}")]
            [ProducesResponseType(StatusCodes.Status204NoContent)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            public async Task<IActionResult> DeleteExampleItem(
                Guid itemId,
                CancellationToken cancellationToken)
            {
                var command = new DeleteExampleItemCommand(itemId);
                var result = await Sender.Send(command, cancellationToken);

                if (result.IsFailure)
                {
                    return HandleFailure(result);
                }

                return NoContent();
            }
        }
}
