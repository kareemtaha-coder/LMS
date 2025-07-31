using LMS.Application.Abstractions.Messaging;
using LMS.Domain.Abstractions;
using LMS.Domain.Curriculums;
using LMS.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.Lessons.UpdateLessonTitle
{
    internal sealed class UpdateLessonTitleCommandHandler : ICommandHandler<UpdateLessonTitleCommand, Result>
    {
        private readonly ICurriculumRepository _curriculumRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateLessonTitleCommandHandler(ICurriculumRepository curriculumRepository, IUnitOfWork unitOfWork)
        {
            _curriculumRepository = curriculumRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateLessonTitleCommand request, CancellationToken cancellationToken)
        {
            var curriculum = await _curriculumRepository.GetByLessonIdAsync(request.LessonId, cancellationToken);
            if (curriculum is null)
            {
                return Result.Failure(CurriculumErrors.LessonNotFound);
            }

            var titleResult = Title.Create(request.Title);
            if (titleResult.IsFailure) return titleResult;

            var updateResult = curriculum.UpdateLessonTitle(request.LessonId, titleResult.Value);
            if (updateResult.IsFailure) return updateResult;

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}