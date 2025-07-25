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

namespace LMS.Application.Features.Curriculums.GetAllCurriculums
{
    internal sealed class GetAllCurriculumsQueryHandler
     : IQueryHandler<GetAllCurriculumsQuery, Result<IReadOnlyList<CurriculumSummaryResponse>>>
    {
        private readonly IApplicationDbContext _context;

        public GetAllCurriculumsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<IReadOnlyList<CurriculumSummaryResponse>>> Handle(
     GetAllCurriculumsQuery request,
     CancellationToken cancellationToken)
        {
            var curriculums = await _context.Curriculums
                .AsNoTracking()
                // Manually select and create the response object.
                .Select(c => new CurriculumSummaryResponse(
                    c.Id,
                    c.Title.Value // Explicitly access the .Value property here.
                ))
                .ToListAsync(cancellationToken);

            return curriculums;
        }
    }
}