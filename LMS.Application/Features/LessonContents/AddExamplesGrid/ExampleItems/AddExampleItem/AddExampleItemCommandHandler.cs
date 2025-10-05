using LMS.Application.Abstractions.Messaging;
using LMS.Domain.Abstractions;
using LMS.Domain.Curriculums;
using LMS.Domain.Lessons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.LessonContents.AddExamplesGrid.ExampleItems.AddExampleItem
{
    internal sealed class AddExampleItemCommandHandler : ICommandHandler<AddExampleItemCommand, Result<Guid>>
    {
        private readonly ICurriculumRepository _curriculumRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddExampleItemCommandHandler(ICurriculumRepository curriculumRepository, IUnitOfWork unitOfWork)
        {
            _curriculumRepository = curriculumRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(AddExampleItemCommand request, CancellationToken cancellationToken)
        {
            var curriculum = await _curriculumRepository.GetByContentIdAsync(request.ContentId, cancellationToken);
            if (curriculum is null)
            {
                return Result.Failure<Guid>(LessonErrors.ContentNotFound);
            }

            var result = curriculum.AddItemToExamplesGridInLesson(request.ContentId, request.ImageUrl, request.AudioUrl);

            if (result.IsFailure)
            {
                return Result.Failure<Guid>(result.Error);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Find the ID of the newly created item
            var grid = curriculum.Chapters
                .SelectMany(c => c.Lessons)
                .SelectMany(l => l.Contents)
                .OfType<ExamplesGridContent>()
                .First(g => g.Id == request.ContentId);

            var newItem = grid.ExampleItems.First(i => i.ImageUrl == request.ImageUrl);

            return newItem.Id;
        }
    }
}
