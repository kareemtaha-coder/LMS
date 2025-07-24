using LMS.Application.Abstractions.Messaging;
using LMS.Domain.Abstractions;
using LMS.Domain.Curriculums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.Curriculums.UpdateCurriculum
{
    internal sealed class UpdateCurriculumHandler : ICommandHandler<UpdateCurriculumCommand, Result>
    {
        private readonly ICurriculumRepository _curriculumRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCurriculumHandler(ICurriculumRepository curriculumRepository, IUnitOfWork unitOfWork)
        {
            _curriculumRepository = curriculumRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateCurriculumCommand request, CancellationToken cancellationToken)
        {
            // 1. جلب الكيان من قاعدة البيانات
            var curriculum = await _curriculumRepository.GetByIdAsync(request.CurriculumId, cancellationToken);

            if (curriculum is null)
            {
                return Result.Failure(new Error("Curriculum.NotFound", "The curriculum was not found."));
            }

            // 2. استدعاء دالة التحديث في الـ Domain Model
            var updateResult = curriculum.Update(request.Title, request.Introduction);

            if (updateResult.IsFailure)
            {
                return updateResult; // إرجاع خطأ الـ business rule
            }

            // 3. حفظ التغييرات
            // لسنا بحاجة لاستدعاء دالة Update من الـ Repository،
            // لأن EF Core Change Tracker يكتشف التعديلات تلقائيًا.
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}