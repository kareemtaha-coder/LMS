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

namespace LMS.Application.Features.Curriculums.Chapters.DeleteChapter
{
    internal sealed class DeleteChapterCommandHandler : ICommandHandler<DeleteChapterCommand, Result>
    {
        private readonly ICurriculumRepository _curriculumRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;

        public DeleteChapterCommandHandler(
            ICurriculumRepository curriculumRepository,
            IUnitOfWork unitOfWork,
            IFileService fileService)
        {
            _curriculumRepository = curriculumRepository;
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task<Result> Handle(DeleteChapterCommand request, CancellationToken cancellationToken)
        {
            var curriculum = await _curriculumRepository.GetByChapterIdAsync(request.ChapterId, cancellationToken);
            if (curriculum is null)
            {
                // The chapter doesn't exist, so the operation is effectively successful.
                return Result.Success();
            }

            var deletionResult = curriculum.DeleteChapter(request.ChapterId);
            if (deletionResult.IsFailure)
            {
                return deletionResult;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // After successful DB delete, clean up all associated files.
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
