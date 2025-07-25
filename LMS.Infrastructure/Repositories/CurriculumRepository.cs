using LMS.Domain.Curriculums;
using LMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Infrastructure.Repositories
{
    internal sealed class CurriculumRepository : ICurriculumRepository
    {
        private readonly ApplicationDbContext _context;

        public CurriculumRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Curriculum curriculum)
        {
            _context.Curriculums.Add(curriculum);
        }

        public Task<Curriculum?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            // We use .Include() to eagerly load the Chapters collection.
            // This is crucial for the AddChapter command handler, as it needs to
            // operate on the full Curriculum aggregate root.
            return _context.Curriculums
                .Include(c => c.Chapters)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }
    }
}
