using LMS.Application.Abstractions.Messaging;
using LMS.Domain.Abstractions;
using LMS.Domain.Curriculums;
using LMS.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Curriculums.CreateCurriculum
{
    public class CreateCurriculumCommandHandler : ICommandHandler<CreateCurriculumCommand, Guid>
    {
        private readonly ICurriculumRepository curriculumRepository;
        private readonly IUnitOfWork unitOfWork;

        public CreateCurriculumCommandHandler(ICurriculumRepository curriculumRepository, IUnitOfWork unitOfWork)
        {
            this.curriculumRepository = curriculumRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateCurriculumCommand request, CancellationToken cancellationToken)
        {

            var title = new Title(request.Title);
            var introduction = new Introduction(request.Introduction);
            var curriculum = Curriculum.Create(introduction, title);

            curriculumRepository.Add(curriculum);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return curriculum.Id;
        }
    }
}
