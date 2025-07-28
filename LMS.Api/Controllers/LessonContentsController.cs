using LMS.Application.Abstractions.Services;
using LMS.Application.Features.LessonContents.AddExamplesGrid;
using LMS.Application.Features.LessonContents.AddImageWithCaption;
using LMS.Application.Features.LessonContents.AddRichTextContent;
using LMS.Application.Features.LessonContents.AddVideoContent;
using LMS.Application.Features.LessonContents.ReorderLessonContents;
using LMS.Application.Features.LessonContents.UpdateImageWithCaption;
using LMS.Application.Features.LessonContents.UpdateRichTextContent;
using LMS.Application.Features.LessonContents.UpdateVideoContent;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Api.Controllers
{
    [Route("api/lessons/{lessonId:guid}/contents")]
    [ApiController]
    public class LessonContentsController : ApiControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileService _fileService;

        public LessonContentsController(ISender sender, IWebHostEnvironment webHostEnvironment, IFileService fileService) : base(sender)
        {
             _webHostEnvironment = webHostEnvironment;
            _fileService = fileService;
        }

        [HttpPost("rich-text")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddRichTextContent(
            Guid lessonId,
            [FromBody] AddRichTextContentRequest request,
            CancellationToken cancellationToken)
        {
            var command = new AddRichTextContentCommand(
                lessonId,
                request.SortOrder,
                request.ArabicText,
                request.EnglishText,
                request.NoteType);

            var result = await Sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return HandleFailure(result);
            }

            return Ok(result.Value);
        }


            [HttpPost("video")]
            [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            public async Task<IActionResult> AddVideoContent(
                Guid lessonId,
                [FromBody] AddVideoContentRequest request,
                CancellationToken cancellationToken)
            {
                var command = new AddVideoContentCommand(
                    lessonId,
                    request.SortOrder,
                    request.VideoUrl);

                var result = await Sender.Send(command, cancellationToken);

                if (result.IsFailure)
                {
                    return HandleFailure(result);
                }

                return Ok(result.Value);
            }


        [HttpPost("image-with-caption")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddImageWithCaption(
        Guid lessonId,
        [FromForm] AddImageWithCaptionRequest request, // Use [FromForm] for file uploads
        CancellationToken cancellationToken)
        {
            // 1. Validate the incoming file
            if (request.ImageFile is null || request.ImageFile.Length == 0)
            {
                return BadRequest("Image file is required.");
            }

            // 2. Process and save the file
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "lessons");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = $"{Guid.NewGuid()}_{request.ImageFile.FileName}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            await using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await request.ImageFile.CopyToAsync(fileStream, cancellationToken);
            }

            // 3. Create the public URL to be stored in the database
            var imageUrl = $"/images/lessons/{uniqueFileName}";

            // 4. Create the command with the URL and send it to the handler
            var command = new AddImageWithCaptionCommand(
                lessonId,
                request.SortOrder,
                imageUrl,
                request.Caption);

            var result = await Sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                // If the command fails, you might want to delete the uploaded file
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                return HandleFailure(result);
            }

            return Ok(result.Value);
        }

        [HttpPost("examples-grid")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddExamplesGrid(
        Guid lessonId,
        [FromBody] AddExamplesGridRequest request,
        CancellationToken cancellationToken)
        {
            var command = new AddExamplesGridCommand(
                lessonId,
                request.SortOrder);

            var result = await Sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return HandleFailure(result);
            }

            return Ok(result.Value);
        }

        [HttpPut("reorder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ReorderLessonContents(
        Guid lessonId,
        [FromBody] ReorderLessonContentsRequest request,
        CancellationToken cancellationToken)
        {
            var command = new ReorderLessonContentsCommand(lessonId, request.OrderedContentIds);

            var result = await Sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return HandleFailure(result);
            }

            return NoContent(); // 204 NoContent is the standard response for successful updates.
        }
        [HttpPut("{contentId:guid}/rich-text")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateRichTextContent(
       Guid contentId,
       [FromBody] UpdateRichTextContentRequest request,
       CancellationToken cancellationToken)
        {
            var command = new UpdateRichTextContentCommand(
                contentId,
                request.ArabicText,
                request.EnglishText, request.NoteType);

            var result = await Sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return HandleFailure(result);
            }

            return NoContent();
        }
        [HttpPut("{contentId:guid}/video")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateVideoContent(
        Guid contentId,
        [FromBody] UpdateVideoContentRequest request,
        CancellationToken cancellationToken)
        {
            var command = new UpdateVideoContentCommand(
                contentId,
                request.VideoUrl);

            var result = await Sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return HandleFailure(result);
            }

            return NoContent();
        }
        [HttpPut("{contentId:guid}/image-with-caption")]
        public async Task<IActionResult> UpdateImageWithCaption(
        Guid contentId,
        [FromForm] UpdateImageWithCaptionRequest request,
        CancellationToken cancellationToken)
        {
            string? newImageUrl = null;
            if (request.ImageFile is not null && request.ImageFile.Length > 0)
            {
                newImageUrl = await _fileService.SaveFileAsync(request.ImageFile, "images/lessons", cancellationToken);
            }

            var command = new UpdateImageWithCaptionCommand(contentId, newImageUrl, request.Caption);
            var result = await Sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                // If command fails, delete the newly uploaded file if it exists
                _fileService.DeleteFile(newImageUrl);
                return HandleFailure(result);
            }

            return NoContent();
        }
    }
}
