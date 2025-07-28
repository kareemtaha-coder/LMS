using LMS.Application.Abstractions.Messaging;
using LMS.Domain.Abstractions;
using LMS.Domain.Curriculums;
using LMS.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.LessonContents.AddVideoContent
{
    internal sealed class AddVideoContentCommandHandler : ICommandHandler<AddVideoContentCommand, Result<Guid>>
    {
        private readonly ICurriculumRepository _curriculumRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddVideoContentCommandHandler(ICurriculumRepository curriculumRepository, IUnitOfWork unitOfWork)
        {
            _curriculumRepository = curriculumRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(AddVideoContentCommand request, CancellationToken cancellationToken)
        {
            var curriculum = await _curriculumRepository.GetByLessonIdAsync(request.LessonId, cancellationToken);
            if (curriculum is null)
            {
                return Result.Failure<Guid>(new Error("Lesson.NotFound", "The specified lesson could not be found."));
            }

            var sortOrderResult = SortOrder.Create(request.SortOrder);
            if (sortOrderResult.IsFailure) return Result.Failure<Guid>(sortOrderResult.Error);

            var addContentResult = curriculum.AddVideoContentToLesson(
                request.LessonId,
                sortOrderResult.Value,
                request.VideoUrl);

            if (addContentResult.IsFailure)
            {
                return Result.Failure<Guid>(addContentResult.Error);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var lesson = curriculum.Chapters.SelectMany(c => c.Lessons).First(l => l.Id == request.LessonId);
            var newContent = lesson.Contents.First(lc => lc.SortOrder.Value == request.SortOrder);

            return newContent.Id;
        }
    }
}
