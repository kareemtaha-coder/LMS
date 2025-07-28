using LMS.Application.Abstractions.Messaging;
using LMS.Application.Abstractions.Services;
using LMS.Domain.Abstractions;
using LMS.Domain.Curriculums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.LessonContents.DeleteExampleItem
{
    internal sealed class DeleteExampleItemCommandHandler : ICommandHandler<DeleteExampleItemCommand, Result>
    {
        private readonly ICurriculumRepository _curriculumRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;

        public DeleteExampleItemCommandHandler(
            ICurriculumRepository curriculumRepository,
            IUnitOfWork unitOfWork,
            IFileService fileService)
        {
            _curriculumRepository = curriculumRepository;
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task<Result> Handle(DeleteExampleItemCommand request, CancellationToken cancellationToken)
        {
            var curriculum = await _curriculumRepository.GetByExampleItemIdAsync(request.ItemId, cancellationToken);
            if (curriculum is null)
            {
                return Result.Failure(new Error("ExampleItem.NotFound", "The specified item was not found."));
            }

            var deletionResult = curriculum.DeleteExampleItem(request.ItemId);
            if (deletionResult.IsFailure)
            {
                return deletionResult;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // After successful DB delete, delete the associated files
            var deletedItem = deletionResult.Value;
            _fileService.DeleteFile(deletedItem.ImageUrl);
            _fileService.DeleteFile(deletedItem.AudioUrl);

            return Result.Success();
        }
    }
}
