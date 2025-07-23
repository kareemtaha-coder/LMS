using LMS.Application.Abstractions.Messaging;
using LMS.Domain.Abstractions;
using LMS.Domain.Curriculums;
using LMS.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Curriculums.CreateCurriculum
{
    internal sealed class CreateCurriculumCommandHandler : ICommandHandler<CreateCurriculumCommand, Result<Guid>>
    {
        private readonly ICurriculumRepository _curriculumRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCurriculumCommandHandler(ICurriculumRepository curriculumRepository, IUnitOfWork unitOfWork)
        {
            _curriculumRepository = curriculumRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(CreateCurriculumCommand request, CancellationToken cancellationToken)
        {
            Result<Curriculum> curriculumResult = Curriculum.Create(request.Title, request.Introduction);

            if (curriculumResult.IsFailure)
            {
                return Result.Failure<Guid>(curriculumResult.Error);
            }

            _curriculumRepository.Add(curriculumResult.Value);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return curriculumResult.Value.Id;
        }
    }
}

