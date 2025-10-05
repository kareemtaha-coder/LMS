using LMS.Application.Abstractions.Messaging;
using LMS.Domain.Abstractions;
using LMS.Domain.Curriculums;
using LMS.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.LessonContents.AddExamplesGrid
{
    internal sealed class AddExamplesGridCommandHandler : ICommandHandler<AddExamplesGridCommand, Result<Guid>>
    {
        private readonly ICurriculumRepository _curriculumRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddExamplesGridCommandHandler(ICurriculumRepository curriculumRepository, IUnitOfWork unitOfWork)
        {
            _curriculumRepository = curriculumRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(AddExamplesGridCommand request, CancellationToken cancellationToken)
        {
            var curriculum = await _curriculumRepository.GetByLessonIdAsync(request.LessonId, cancellationToken);
            if (curriculum is null)
            {
                return Result.Failure<Guid>(new Error("Lesson.NotFound", "The specified lesson could not be found."));
            }

            var sortOrderResult = SortOrder.Create(request.SortOrder);
            if (sortOrderResult.IsFailure) return Result.Failure<Guid>(sortOrderResult.Error);

            Title? title = null;
            if (!string.IsNullOrEmpty(request.Title))
            {
                var titleResult = Title.Create(request.Title);
                if (titleResult.IsFailure) return Result.Failure<Guid>(titleResult.Error);
                title = titleResult.Value;
            }

            var addContentResult = curriculum.AddExamplesGridToLesson(
                request.LessonId,
                sortOrderResult.Value,
                title!);

            if (addContentResult.IsFailure)
            {
                return Result.Failure<Guid>(addContentResult.Error);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // ... find and return newContent.Id ...
            var lesson = curriculum.Chapters.SelectMany(c => c.Lessons).First(l => l.Id == request.LessonId);
            var newContent = lesson.Contents.First(lc => lc.SortOrder.Value == request.SortOrder);
            return newContent.Id;
        }
    }
}