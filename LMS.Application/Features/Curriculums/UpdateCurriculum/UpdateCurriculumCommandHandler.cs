using LMS.Application.Abstractions.Messaging;
using LMS.Domain.Abstractions;
using LMS.Domain.Curriculums;
using LMS.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.Curriculums.UpdateCurriculum
{
    internal sealed class UpdateCurriculumCommandHandler : ICommandHandler<UpdateCurriculumCommand, Result>
    {
        private readonly ICurriculumRepository _curriculumRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCurriculumCommandHandler(ICurriculumRepository curriculumRepository, IUnitOfWork unitOfWork)
        {
            _curriculumRepository = curriculumRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateCurriculumCommand request, CancellationToken cancellationToken)
        {
            var curriculum = await _curriculumRepository.GetByIdAsync(request.CurriculumId, cancellationToken);
            if (curriculum is null)
            {
                return Result.Failure(new Error("Curriculum.NotFound", "The specified curriculum was not found."));
            }

            var titleResult = Title.Create(request.Title);
            if (titleResult.IsFailure) return titleResult;

            var introductionResult = Introduction.Create(request.Introduction);
            if (introductionResult.IsFailure) return introductionResult;

            var updateResult = curriculum.Update(titleResult.Value, introductionResult.Value);
            if (updateResult.IsFailure) return updateResult;

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
