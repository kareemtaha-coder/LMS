using LMS.Application.Abstractions.Messaging;
using LMS.Domain.Abstractions;
using LMS.Domain.Curriculums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.Lessons.UnpublishLesson
{
    internal sealed class UnpublishLessonCommandHandler : ICommandHandler<UnpublishLessonCommand, Result>
    {
        private readonly ICurriculumRepository _curriculumRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UnpublishLessonCommandHandler(ICurriculumRepository curriculumRepository, IUnitOfWork unitOfWork)
        {
            _curriculumRepository = curriculumRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UnpublishLessonCommand request, CancellationToken cancellationToken)
        {
            // 1. Load the aggregate root that contains the lesson.
            var curriculum = await _curriculumRepository.GetByLessonIdAsync(request.LessonId, cancellationToken);

            // 2. If the curriculum is not found, it means the lesson doesn't exist.
            // We use the correct error from the Curriculum domain.
            if (curriculum is null)
            {
                return Result.Failure(CurriculumErrors.LessonNotFound);
            }

            // 3. Delegate the business logic to the domain model.
            var result = curriculum.UnpublishLesson(request.LessonId);
            if (result.IsFailure)
            {
                return result;
            }

            // 4. Save the changes to the database.
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}