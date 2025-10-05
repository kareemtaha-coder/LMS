using LMS.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.Curriculums
{
    public interface ICurriculumRepository : IRepository<Curriculum>
    {
        public Task<Curriculum?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        public void Add(Curriculum curriculum);

        Task<Curriculum?> GetByChapterIdAsync(Guid chapterId, CancellationToken cancellationToken = default);

        Task<Curriculum?> GetByLessonIdAsync(Guid lessonId, CancellationToken cancellationToken = default);
        Task<Curriculum?> GetByContentIdAsync(Guid contentId, CancellationToken cancellationToken = default);
        Task<Curriculum?> GetByExampleItemIdAsync(Guid itemId, CancellationToken cancellationToken = default);

    }
}
