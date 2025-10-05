using LMS.Application.Abstractions.Messaging;
using LMS.Domain.Abstractions;
using LMS.Domain.Curriculums;
using LMS.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.Curriculums.Chapters.UpdateChapter
{
    internal sealed class UpdateChapterCommandHandler : ICommandHandler<UpdateChapterCommand, Result>
    {
        private readonly ICurriculumRepository _curriculumRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateChapterCommandHandler(ICurriculumRepository curriculumRepository, IUnitOfWork unitOfWork)
        {
            _curriculumRepository = curriculumRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateChapterCommand request, CancellationToken cancellationToken)
        {
            var curriculum = await _curriculumRepository.GetByChapterIdAsync(request.ChapterId, cancellationToken);
            if (curriculum is null)
            {
                return Result.Failure(CurriculumErrors.ChapterNotFound);
            }

            var titleResult = Title.Create(request.Title);
            if (titleResult.IsFailure) return titleResult;

            var updateResult = curriculum.UpdateChapterTitle(request.ChapterId, titleResult.Value);
            if (updateResult.IsFailure) return updateResult;

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
