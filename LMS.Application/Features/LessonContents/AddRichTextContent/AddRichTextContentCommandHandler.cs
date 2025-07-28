using LMS.Application.Abstractions.Messaging;
using LMS.Domain.Abstractions;
using LMS.Domain.Curriculums;
using LMS.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.LessonContents.AddRichTextContent
{
    internal sealed class AddRichTextContentCommandHandler : ICommandHandler<AddRichTextContentCommand, Result<Guid>>
    {
        private readonly ICurriculumRepository _curriculumRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddRichTextContentCommandHandler(ICurriculumRepository curriculumRepository, IUnitOfWork unitOfWork)
        {
            _curriculumRepository = curriculumRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(AddRichTextContentCommand request, CancellationToken cancellationToken)
        {
            var curriculum = await _curriculumRepository.GetByLessonIdAsync(request.LessonId, cancellationToken);
            if (curriculum is null)
            {
                return Result.Failure<Guid>(new Error("Lesson.NotFound", "The specified lesson could not be found."));
            }

            var sortOrderResult = SortOrder.Create(request.SortOrder);
            if (sortOrderResult.IsFailure) return Result.Failure<Guid>(sortOrderResult.Error);

            // Pass the NoteType from the request to the domain method
            var addContentResult = curriculum.AddRichTextContentToLesson(
                request.LessonId,
                sortOrderResult.Value,
                request.ArabicText,
                request.EnglishText,
                request.NoteType); // The missing parameter is now added

            if (addContentResult.IsFailure)
            {
                return Result.Failure<Guid>(addContentResult.Error);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var lesson = curriculum.Chapters.SelectMany(c => c.Lessons).First(l => l.Id == request.LessonId);
            var newContent = lesson.Contents.First(lc => lc.SortOrder.Value == request.SortOrder);

            return newContent.Id;
        }
    }
}