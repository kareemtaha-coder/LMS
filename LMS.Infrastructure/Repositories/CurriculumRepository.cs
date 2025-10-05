using LMS.Domain.Curriculums;
using LMS.Domain.Lessons;
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

        public Task<Curriculum?> GetByChapterIdAsync(Guid chapterId, CancellationToken cancellationToken = default)
        {
            // This query finds the Curriculum that has a chapter with the matching ID.
            // We also eager-load the necessary collections for the command.
            return _context.Curriculums
                .Include(c => c.Chapters)
                    .ThenInclude(ch => ch.Lessons)
                .FirstOrDefaultAsync(c => c.Chapters.Any(ch => ch.Id == chapterId), cancellationToken);
        }

        public Task<Curriculum?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return _context.Curriculums
                .Include(c => c.Chapters)
                    .ThenInclude(ch => ch.Lessons)
                        .ThenInclude(l => l.Contents)
                            .ThenInclude(co => (co as ExamplesGridContent)!.ExampleItems)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }
        public Task<Curriculum?> GetByLessonIdAsync(Guid lessonId, CancellationToken cancellationToken = default)
        {
            return _context.Curriculums
                .Include(c => c.Chapters)
                    .ThenInclude(ch => ch.Lessons)
                        .ThenInclude(l => l.Contents)
                .FirstOrDefaultAsync(c => c.Chapters.SelectMany(ch => ch.Lessons).Any(l => l.Id == lessonId), cancellationToken);
        }
        public Task<Curriculum?> GetByContentIdAsync(Guid contentId, CancellationToken cancellationToken = default)
        {
            // This query finds the Curriculum that contains the specified content ID.
            return _context.Curriculums
                .Include(c => c.Chapters)
                    .ThenInclude(ch => ch.Lessons)
                        .ThenInclude(l => l.Contents)
                            .ThenInclude(co => (co as ExamplesGridContent)!.ExampleItems) // Eagerly load everything needed
                .FirstOrDefaultAsync(c =>
                    c.Chapters.SelectMany(ch => ch.Lessons)
                              .SelectMany(l => l.Contents)
                              .Any(co => co.Id == contentId),
                    cancellationToken);
        }

        public Task<Curriculum?> GetByExampleItemIdAsync(Guid itemId, CancellationToken cancellationToken = default)
        {
            return _context.Curriculums
                .Include(c => c.Chapters)
                    .ThenInclude(ch => ch.Lessons)
                        .ThenInclude(l => l.Contents)
                            .ThenInclude(co => (co as ExamplesGridContent)!.ExampleItems)
                .FirstOrDefaultAsync(c =>
                    c.Chapters.SelectMany(ch => ch.Lessons)
                              .SelectMany(l => l.Contents.OfType<ExamplesGridContent>())
                              .SelectMany(g => g.ExampleItems)
                              .Any(i => i.Id == itemId),
                    cancellationToken);
        }

        public void Remove(Curriculum curriculum) // Implement the Remove method
        {
            _context.Curriculums.Remove(curriculum);
        }

     
    }
}
