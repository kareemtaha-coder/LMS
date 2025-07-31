using LMS.Application.Abstractions.Messaging;
using LMS.Domain.Abstractions;
using LMS.Domain.Curriculums;
using LMS.Domain.Lessons;
using LMS.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.LessonContents.UpdateVideoContent
{
    internal sealed class UpdateVideoContentCommandHandler : ICommandHandler<UpdateVideoContentCommand, Result>
    {
        private readonly ICurriculumRepository _curriculumRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateVideoContentCommandHandler(ICurriculumRepository curriculumRepository, IUnitOfWork unitOfWork)
        {
            _curriculumRepository = curriculumRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateVideoContentCommand request, CancellationToken cancellationToken)
        {
            var curriculum = await _curriculumRepository.GetByContentIdAsync(request.ContentId, cancellationToken);
            if (curriculum is null)
            {
                return Result.Failure(LessonErrors.ContentNotFound);
            }

            Title title = null;
            if (!string.IsNullOrEmpty(request.Title))
            {
                var titleResult = Title.Create(request.Title);
                if (titleResult.IsFailure) return titleResult;
                title = titleResult.Value;
            }

            var result = curriculum.UpdateVideoContent(request.ContentId, request.VideoUrl, title);

            if (result.IsFailure)
            {
                return result;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
