using LMS.Domain.Chapters;
using LMS.Domain.Curriculums;
using LMS.Domain.Lessons;
using LMS.Domain.Supervisor;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Abstractions.Data
{
    public interface IApplicationDbContext
    {
        /// <summary>
        /// Gets the DbSet for Curriculum aggregates.
        /// </summary>
        DbSet<Curriculum> Curriculums { get; }
        public DbSet<Chapter> Chapter { get; set; }
        public DbSet<Lesson> Lesson { get; set; }


        /// <summary>
        /// Gets the DbSet for Supervisor aggregates.
        /// </summary>
        DbSet<Supervisor> Supervisors { get; }

        // Add other Aggregate Root DbSets here...

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>The number of state entries written to the database.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
