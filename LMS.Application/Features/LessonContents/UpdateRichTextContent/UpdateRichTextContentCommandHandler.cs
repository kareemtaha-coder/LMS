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

namespace LMS.Application.Features.LessonContents.UpdateRichTextContent
{
    internal sealed class UpdateRichTextContentCommandHandler : ICommandHandler<UpdateRichTextContentCommand, Result>
    {
        private readonly ICurriculumRepository _curriculumRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateRichTextContentCommandHandler(ICurriculumRepository curriculumRepository, IUnitOfWork unitOfWork)
        {
            _curriculumRepository = curriculumRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateRichTextContentCommand request, CancellationToken cancellationToken)
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

            // Pass the NoteType from the request to the domain method
            var result = curriculum.UpdateRichTextContent(
                request.ContentId,
                request.ArabicText,
                request.EnglishText,
                request.NoteType,
                title);

            if (result.IsFailure)
            {
                return result;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
