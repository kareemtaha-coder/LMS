using LMS.Application.Abstractions.Data;
using LMS.Application.Abstractions.Messaging;
using LMS.Domain.Abstractions;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.Curriculums.GetCurriculum
{
    internal sealed class GetCurriculumQueryHandler
    : IQueryHandler<GetCurriculumQuery, Result<CurriculumResponse>>
    {
        private readonly IApplicationDbContext _context;

        public GetCurriculumQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<CurriculumResponse>> Handle(
            GetCurriculumQuery request,
            CancellationToken cancellationToken)
        {
            var curriculumResponse = await _context.Curriculums
                .AsNoTracking() // <--- هذا هو الحل
                .Where(c => c.Id == request.CurriculumId)
                .ProjectToType<CurriculumResponse>()
                .FirstOrDefaultAsync(cancellationToken);

            if (curriculumResponse is null)
            {
                return Result.Failure<CurriculumResponse>(new Error(
                    "Curriculum.NotFound",
                    $"The curriculum with ID '{request.CurriculumId}' was not found."));
            }

            return curriculumResponse;
        }
    }
}