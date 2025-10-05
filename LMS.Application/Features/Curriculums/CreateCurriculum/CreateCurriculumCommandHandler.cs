using LMS.Application.Abstractions.Messaging;
using LMS.Domain.Abstractions;
using LMS.Domain.Curriculums;
using LMS.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.Curriculums.CreateCurriculum
{
    internal sealed class CreateCurriculumCommandHandler : ICommandHandler<CreateCurriculumCommand, Result<Guid>>
    {
        private readonly ICurriculumRepository _curriculumRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCurriculumCommandHandler(ICurriculumRepository curriculumRepository, IUnitOfWork unitOfWork)
        {
            _curriculumRepository = curriculumRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(CreateCurriculumCommand request, CancellationToken cancellationToken)
        {
            // 1. إنشاء الـ Value Objects أولاً للتحقق من صحتها
            var titleResult = Title.Create(request.Title);
            if (titleResult.IsFailure)
            {
                return Result.Failure<Guid>(titleResult.Error);
            }

            var introductionResult = Introduction.Create(request.Introduction);
            if (introductionResult.IsFailure)
            {
                return Result.Failure<Guid>(introductionResult.Error);
            }

            // 2. استخدام الـ Factory Method في الـ Aggregate Root لإنشاء الكيان
            // هذا يضمن تطبيق كل قواعد العمل (business rules) الخاصة بالـ Domain
            var curriculumResult = Curriculum.Create(
                request.Title,
                request.Introduction);

            if (curriculumResult.IsFailure)
            {
                return Result.Failure<Guid>(curriculumResult.Error);
            }

            var curriculum = curriculumResult.Value;

            // 3. إضافة الكيان إلى الـ Repository
            _curriculumRepository.Add(curriculum);

            // 4. حفظ التغييرات في قاعدة البيانات
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 5. إعادة معرّف الكيان الجديد كدليل على النجاح
            return curriculum.Id;
        }
    }
}

