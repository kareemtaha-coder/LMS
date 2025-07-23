using LMS.Domain.Abstractions;
using LMS.Domain.Chapters;
using LMS.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.Curriculums
{
    public sealed class Curriculum:Entity
    {
        private readonly List<Chapter> chapters = new();
        public IReadOnlyCollection<Chapter> Chapters => chapters.AsReadOnly();
        public Title Title { get; private set; }
        public Introduction Introduction { get; private set; }

        private Curriculum(Guid id,Introduction introduction,Title title) : base(id)
        {
            Title = title;
            Introduction = introduction;

        }
        private Curriculum() : base(Guid.NewGuid()) { }


        public static Curriculum Create(Introduction introduction, Title title)
        {
            var curriculum = new Curriculum(Guid.NewGuid(), introduction, title);
            return curriculum;
        }

        public void AddChapter(Title title , SortOrder sortOrder)
        {
            if (chapters.Any(c => c.Title.Value == title.Value))
            {
                throw new InvalidOperationException("A chapter with this title already exists.");
            }
            var chapter = Chapter.Create(title, this.Id, sortOrder);
            chapters.Add(chapter);
        }
    }
}
