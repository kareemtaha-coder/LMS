using LMS.Application.Abstractions.Messaging;
using LMS.Application.Abstractions.Services;
using LMS.Domain.Abstractions;
using LMS.Domain.Curriculums;
using LMS.Domain.Lessons;
using LMS.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.LessonContents.UpdateImageWithCaption
{
    internal sealed class UpdateImageWithCaptionCommandHandler : ICommandHandler<UpdateImageWithCaptionCommand, Result>
    {
        private readonly ICurriculumRepository _curriculumRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;

        public UpdateImageWithCaptionCommandHandler(
            ICurriculumRepository curriculumRepository,
            IUnitOfWork unitOfWork,
            IFileService fileService) // Inject the service
        {
            _curriculumRepository = curriculumRepository;
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task<Result> Handle(UpdateImageWithCaptionCommand request, CancellationToken cancellationToken)
        {
            var curriculum = await _curriculumRepository.GetByContentIdAsync(request.ContentId, cancellationToken);
            if (curriculum is null) return Result.Failure(LessonErrors.ContentNotFound);

            Title title = null;
            if (!string.IsNullOrEmpty(request.Title))
            {
                var titleResult = Title.Create(request.Title);
                if (titleResult.IsFailure) return titleResult;
                title = titleResult.Value;
            }


            var content = curriculum.Chapters
                .SelectMany(c => c.Lessons)
                .SelectMany(l => l.Contents)
                .FirstOrDefault(co => co.Id == request.ContentId);

            if (content is not ImageWithCaptionContent imageContent)
            {
                return Result.Failure(LessonErrors.InvalidContentType);
            }

            string oldImageUrl = imageContent.ImageUrl;
            string finalImageUrl = request.NewImageUrl ?? oldImageUrl;

            var result = curriculum.UpdateImageWithCaptionContent(request.ContentId, finalImageUrl, request.Caption,title);
            if (result.IsFailure) return result;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // After DB save, delete the old file if a new one was provided
            if (request.NewImageUrl is not null && oldImageUrl != request.NewImageUrl)
            {
                _fileService.DeleteFile(oldImageUrl);
            }

            return Result.Success();
        }
    }
}
