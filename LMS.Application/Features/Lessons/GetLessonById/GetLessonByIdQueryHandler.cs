using LMS.Application.Abstractions.Data;
using LMS.Application.Abstractions.Messaging;
using LMS.Domain.Abstractions;
using LMS.Domain.Lessons;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.Lessons.GetLessonById
{
    internal sealed class GetLessonByIdQueryHandler : IQueryHandler<GetLessonByIdQuery, Result<LessonResponse>>
    {
        private readonly IApplicationDbContext _context;

        public GetLessonByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<LessonResponse>> Handle(GetLessonByIdQuery request, CancellationToken cancellationToken)
        {
            // Step 1: Fetch the lesson and its top-level contents.
            var lesson = await _context.Lesson
                .Include(l => l.Contents)
                .FirstOrDefaultAsync(l => l.Id == request.LessonId, cancellationToken);

            if (lesson is null)
            {
                return Result.Failure<LessonResponse>(new Error("Lesson.NotFound", "The specified lesson was not found."));
            }

            // Step 2: Explicitly load the ExampleItems using the DbSet.
            // This code does not use .Entry() and is the correct approach.
            var gridContentIds = lesson.Contents
                .OfType<ExamplesGridContent>()
                .Select(c => c.Id)
                .ToList();

            if (gridContentIds.Any())
            {
                await _context.LessonContent
                    .OfType<ExamplesGridContent>()
                    .Where(c => gridContentIds.Contains(c.Id))
                    .Include(c => c.ExampleItems)
                    .LoadAsync(cancellationToken);
            }


            // Step 3: Map to the response DTO.
            var contentResponses = lesson.Contents
                .OrderBy(c => c.SortOrder.Value)
                .Select(content =>
                {
                    LessonContentResponse response = content switch
                    {
                        RichTextContent rtc => new RichTextContentResponse(rtc.Id, rtc.SortOrder.Value, rtc.ArabicText, rtc.EnglishText),
                        VideoContent vc => new VideoContentResponse(vc.Id, vc.SortOrder.Value, vc.VideoUrl),
                        ImageWithCaptionContent iwc => new ImageWithCaptionContentResponse(iwc.Id, iwc.SortOrder.Value, iwc.ImageUrl, iwc.Caption),
                        ExamplesGridContent egc => new ExamplesGridContentResponse(
                            egc.Id,
                            egc.SortOrder.Value,
                            egc.ExampleItems.Select(ei => new ExampleItemResponse(ei.Id, ei.ImageUrl, ei.AudioUrl)).ToList()
                        ),
                        _ => throw new InvalidOperationException("Unknown content type")
                    };
                    return response;
                })
                .ToList();

            var lessonResponse = new LessonResponse(
                lesson.Id,
                lesson.Title.Value,
                lesson.SortOrder.Value,
                contentResponses);

            return lessonResponse;
        }
    }
}
