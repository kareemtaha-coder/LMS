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
    internal sealed class GetAllCurriculumsHandler
    : IQueryHandler<GetAllCurriculumsQuery, Result<IReadOnlyCollection<CurriculumSummaryResponse>>>
    {
        private readonly IApplicationDbContext _context;

        public GetAllCurriculumsHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<IReadOnlyCollection<CurriculumSummaryResponse>>> Handle(
            GetAllCurriculumsQuery request,
            CancellationToken cancellationToken)
        {
            // الكود أصبح أبسط بكثير:
            // 1. جلب البيانات.
            // 2. ترتيبها.
            // 3. تحويلها.
            var curriculumSummaries = await _context.Curriculums
                .AsNoTracking()
                .OrderBy(c => c.Title.Value)
                .ProjectToType<CurriculumSummaryResponse>()
                .ToListAsync(cancellationToken);

            return curriculumSummaries;
        }
    }
}