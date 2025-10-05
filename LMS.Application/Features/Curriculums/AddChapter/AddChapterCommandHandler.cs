using LMS.Application.Abstractions.Data;
using LMS.Application.Abstractions.Messaging;
using LMS.Domain.Abstractions;
using LMS.Domain.Curriculums;
using LMS.Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.Curriculums.AddChapter
{

    internal sealed class AddChapterCommandHandler : ICommandHandler<AddChapterCommand, Result<Guid>>
    {
        private readonly ICurriculumRepository _curriculumRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddChapterCommandHandler(ICurriculumRepository curriculumRepository, IUnitOfWork unitOfWork)
        {
            _curriculumRepository = curriculumRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(AddChapterCommand request, CancellationToken cancellationToken)
        {
            var curriculum = await _curriculumRepository.GetByIdAsync(request.CurriculumId, cancellationToken);
            if (curriculum is null)
            {
                return Result.Failure<Guid>(new Error("Curriculum.NotFound", "The specified curriculum was not found."));
            }

            // Create Value Objects and check for failures
            var titleResult = Title.Create(request.Title);
            if (titleResult.IsFailure)
            {
                return Result.Failure<Guid>(titleResult.Error);
            }

            // **This is the updated part**
            var sortOrderResult = SortOrder.Create(request.SortOrder);
            if (sortOrderResult.IsFailure)
            {
                return Result.Failure<Guid>(sortOrderResult.Error);
            }

            // Delegate to the domain entity using the created value objects
            var addChapterResult = curriculum.AddChapter(titleResult.Value, sortOrderResult.Value);
            if (addChapterResult.IsFailure)
            {
                return Result.Failure<Guid>(addChapterResult.Error);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var newChapter = curriculum.Chapters.FirstOrDefault(c => c.Title == titleResult.Value);
            if (newChapter is null)
            {
                return Result.Failure<Guid>(new Error("Chapter.NotFoundAfterCreation", "Could not find the chapter after creation."));
            }

            return newChapter.Id;
        }
    }
}