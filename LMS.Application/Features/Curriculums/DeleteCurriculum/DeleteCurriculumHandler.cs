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

namespace LMS.Application.Features.Curriculums.DeleteCurriculum
{
    internal sealed class DeleteCurriculumCommandHandler : ICommandHandler<DeleteCurriculumCommand, Result>
    {
        private readonly ICurriculumRepository _curriculumRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;

        public DeleteCurriculumCommandHandler(
            ICurriculumRepository curriculumRepository,
            IUnitOfWork unitOfWork,
            IFileService fileService)
        {
            _curriculumRepository = curriculumRepository;
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task<Result> Handle(DeleteCurriculumCommand request, CancellationToken cancellationToken)
        {
            // Load the entire aggregate root with all nested collections
            var curriculum = await _curriculumRepository.GetByIdAsync(request.CurriculumId, cancellationToken);
            if (curriculum is null)
            {
                return Result.Success(); // Already deleted
            }

            // 1. Collect all file paths before deleting from the database
            var filePathsToDelete = curriculum.Chapters
                .SelectMany(ch => ch.Lessons)
                .SelectMany(l => l.Contents)
                .SelectMany(co =>
                {
                    var paths = new List<string?>();
                    if (co is ImageWithCaptionContent iwc)
                    {
                        paths.Add(iwc.ImageUrl);
                    }
                    else if (co is ExamplesGridContent egc)
                    {
                        paths.AddRange(egc.ExampleItems.Select(i => i.ImageUrl));
                        paths.AddRange(egc.ExampleItems.Select(i => i.AudioUrl));
                    }
                    return paths;
                })
                .Where(path => !string.IsNullOrEmpty(path))
                .ToList();

            // 2. Remove the aggregate root from the database
            _curriculumRepository.Remove(curriculum);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 3. After successful DB deletion, delete all collected files
            foreach (var path in filePathsToDelete)
            {
                _fileService.DeleteFile(path);
            }

            return Result.Success();
        }
    }
}