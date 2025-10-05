using LMS.Application.Abstractions.Data;
using LMS.Application.Abstractions.Messaging;
using LMS.Domain.Abstractions;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.Curriculums.GetCurriculum
{
    public class GetCurriculumByIdQueryHandler : IRequestHandler<GetCurriculumQuery, Result<CurriculumResponse>>
    {
        private readonly IApplicationDbContext _context;

        public GetCurriculumByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<CurriculumResponse>> Handle(GetCurriculumQuery request, CancellationToken cancellationToken)
        {
            // Query the database to find the curriculum.
            // We use .AsNoTracking() for read-only queries for better performance.
            // We use Include and ThenInclude to load the related chapters and lessons.
            var curriculum = await _context.Curriculums
                .AsNoTracking()
                .Include(c => c.Chapters)
                    .ThenInclude(ch => ch.Lessons)
                .FirstOrDefaultAsync(c => c.Id == request.CurriculumId, cancellationToken);

            if (curriculum is null)
            {
                // If not found, return a failure Result with an error.
                return Result.Failure<CurriculumResponse>(new Error(
                    "Curriculum.NotFound",
                    $"The curriculum with ID '{request.CurriculumId}' was not found."));
            }

            // Manually map the domain entity to our response DTO.
            // This ensures a clean separation.
            var response = new CurriculumResponse(
                curriculum.Id,
                curriculum.Title.Value, // Assuming Title is a Value Object
                curriculum.Introduction.Value, // Assuming Introduction is a Value Object
                curriculum.Chapters.Select(ch => new ChapterResponse(
                    ch.Id,
                    ch.Title.Value,
                    ch.SortOrder.Value,
                    ch.Lessons.Select(l => new LessonResponse(
                        l.Id,
                        l.Title.Value,
                        l.SortOrder.Value
                    )).ToList()
                )).ToList()
            );

            // Return a success Result containing the DTO.
            return Result.Success(response);
        }

       
    }
}