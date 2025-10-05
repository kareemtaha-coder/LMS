using LMS.Application.Abstractions.Data;
using LMS.Application.Abstractions.Messaging;
using LMS.Application.Features.Shared;
using LMS.Domain.Abstractions;
using LMS.Domain.Lessons;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.Lessons.GetAllLessonsInChapter
{
    internal sealed class GetAllLessonsInChapterQueryHandler
    : IQueryHandler<GetAllLessonsInChapterQuery, Result<IReadOnlyList<LessonSummaryResponse>>>
    {
        private readonly IApplicationDbContext _context;

        public GetAllLessonsInChapterQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<IReadOnlyList<LessonSummaryResponse>>> Handle(
            GetAllLessonsInChapterQuery request,
            CancellationToken cancellationToken)
        {
            // Start building the query
            var query = _context.Lesson
                .AsNoTracking()
                .Where(l => l.ChapterId == request.ChapterId);

            // **This is the key:** Conditionally add the filter
            if (request.Status.HasValue)
            {
                query = query.Where(l => l.Status == request.Status.Value);
            }

            // Execute the final query
            var lessons = await query
                .OrderBy(l => l.SortOrder.Value)
                .Select(l => new LessonSummaryResponse(
                    l.Id,
                    l.Title.Value,
                    l.SortOrder.Value))
                .ToListAsync(cancellationToken);

            return lessons;
        }
    }
}