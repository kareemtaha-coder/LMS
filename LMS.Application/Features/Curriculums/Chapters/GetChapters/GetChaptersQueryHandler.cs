using LMS.Application.Abstractions.Data;
using LMS.Application.Abstractions.Messaging;
using LMS.Domain.Abstractions;
using LMS.Domain.Chapters;
using LMS.Domain.Curriculums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.Curriculums.Chapters.GetChapters
{

    public sealed class GetChaptersQueryHandler
        : IQueryHandler<GetChaptersQuery, Result<List<ChapterResponse>>>
    {
        private readonly IApplicationDbContext _dbContext;

        public GetChaptersQueryHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<List<ChapterResponse>>> Handle(
            GetChaptersQuery request,
            CancellationToken cancellationToken)
        {
            // Fix the issue by removing the parentheses after 'Curriculums' since it is a property, not a method.
            var curriculumExists = await _dbContext.Curriculums
                .AnyAsync(c => c.Id == request.CurriculumId, cancellationToken);

            if (!curriculumExists)
            {
                return Result.Failure<List<ChapterResponse>>(CurriculumErrors.CurriculumNotFound);
            }

            // Query the chapters, order them, and project them into the DTO.
            var chapters = await _dbContext.Chapter
                .Where(c => c.CurriculumId == request.CurriculumId)
                .OrderBy(c => c.SortOrder.Value)
                .AsNoTracking()
                .Select(c => new ChapterResponse(
                    c.Id,
                    c.Title.Value,
                    c.SortOrder.Value))
                .ToListAsync(cancellationToken);

            return chapters;
        }
    }
}