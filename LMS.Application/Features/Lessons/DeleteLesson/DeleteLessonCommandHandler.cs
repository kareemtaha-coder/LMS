using LMS.Application.Abstractions.Messaging;
using LMS.Application.Abstractions.Services;
using LMS.Domain.Abstractions;
using LMS.Domain.Curriculums;
using LMS.Domain.Lessons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.Lessons.DeleteLesson
{
    internal sealed class DeleteLessonCommandHandler : ICommandHandler<DeleteLessonCommand, Result>
    {
        private readonly ICurriculumRepository _curriculumRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;

        public DeleteLessonCommandHandler(
            ICurriculumRepository curriculumRepository,
            IUnitOfWork unitOfWork,
            IFileService fileService)
        {
            _curriculumRepository = curriculumRepository;
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task<Result> Handle(DeleteLessonCommand request, CancellationToken cancellationToken)
        {
            var curriculum = await _curriculumRepository.GetByLessonIdAsync(request.LessonId, cancellationToken);
            if (curriculum is null)
            {
                // If not found, it might have been already deleted, which is a success from the user's perspective.
                return Result.Success();
            }

            var deletionResult = curriculum.DeleteLesson(request.LessonId);
            if (deletionResult.IsFailure)
            {
                return deletionResult;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // After DB save is successful, clean up all associated files.
            var contentsToDelete = deletionResult.Value;
            foreach (var content in contentsToDelete)
            {
                switch (content)
                {
                    case ImageWithCaptionContent iwc:
                        _fileService.DeleteFile(iwc.ImageUrl);
                        break;
                    case ExamplesGridContent egc:
                        foreach (var item in egc.ExampleItems)
                        {
                            _fileService.DeleteFile(item.ImageUrl);
                            _fileService.DeleteFile(item.AudioUrl);
                        }
                        break;
                }
            }

            return Result.Success();
        }
    }
}
