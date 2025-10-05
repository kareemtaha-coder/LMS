using LMS.Application.Abstractions.Data;
using LMS.Application.Abstractions.Messaging;
using LMS.Application.Features.Shared;
using LMS.Domain.Abstractions;
using LMS.Domain.Chapters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.Curriculums.Chapters.GetChapterById
{
    internal sealed class GetChapterByIdQueryHandler
     : IQueryHandler<GetChapterByIdQuery, Result<ChapterWithLessonsResponse>>
    {
        private readonly IApplicationDbContext _dbContext;

        public GetChapterByIdQueryHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<ChapterWithLessonsResponse>> Handle(
            GetChapterByIdQuery request,
            CancellationToken cancellationToken)
        {
            // 1. Query the database for the chapter, including its lessons.
            // We use AsNoTracking() for better performance on read-only operations.
            var chapter = await _dbContext.Chapter
                .Include(c => c.Lessons)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == request.ChapterId, cancellationToken);

            if (chapter is null)
            {
                return Result.Failure<ChapterWithLessonsResponse>(ChapterErrors.NotFound);
            }

            // 2. Manually map the entity to our response DTOs.
            var response = new ChapterWithLessonsResponse(
                chapter.Id,
                chapter.Title.Value,
                chapter.SortOrder.Value,
                chapter.Lessons.OrderBy(l => l.SortOrder.Value)
                    .Select(lesson => new LessonSummaryResponse(
                        lesson.Id,
                        lesson.Title.Value,
                        lesson.SortOrder.Value))
                    .ToList());

            // 3. Return the successful result.
            return response;
        }
    }
}