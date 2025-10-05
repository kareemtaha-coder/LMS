using LMS.Application.Abstractions.Messaging;
using LMS.Domain.Abstractions;
using LMS.Domain.Curriculums;
using LMS.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.Lessons.AddLessonToChapter
{
    internal sealed class AddLessonToChapterCommandHandler : ICommandHandler<AddLessonToChapterCommand, Result<Guid>>
    {
        private readonly ICurriculumRepository _curriculumRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddLessonToChapterCommandHandler(ICurriculumRepository curriculumRepository, IUnitOfWork unitOfWork)
        {
            _curriculumRepository = curriculumRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(AddLessonToChapterCommand request, CancellationToken cancellationToken)
        {
            // 1. Use the new repository method to find and load the correct aggregate root.
            var curriculum = await _curriculumRepository.GetByChapterIdAsync(request.ChapterId, cancellationToken);
            if (curriculum is null)
            {
                return Result.Failure<Guid>(CurriculumErrors.ChapterNotFound);
            }

            // 2. Create Value Objects
            var titleResult = Title.Create(request.Title);
            if (titleResult.IsFailure) return Result.Failure<Guid>(titleResult.Error);

            var sortOrderResult = SortOrder.Create(request.SortOrder);
            if (sortOrderResult.IsFailure) return Result.Failure<Guid>(sortOrderResult.Error);

            // 3. Delegate the operation to the domain model's aggregate root.
            var addLessonResult = curriculum.AddLessonToChapter(
                request.ChapterId,
                titleResult.Value,
                sortOrderResult.Value);

            if (addLessonResult.IsFailure)
            {
                return Result.Failure<Guid>(addLessonResult.Error);
            }

            // 4. Save changes
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 5. Find the new lesson to return its ID
            var chapter = curriculum.Chapters.First(c => c.Id == request.ChapterId);
            var newLesson = chapter.Lessons.First(l => l.Title == titleResult.Value);

            return newLesson.Id;
        }
    }
}