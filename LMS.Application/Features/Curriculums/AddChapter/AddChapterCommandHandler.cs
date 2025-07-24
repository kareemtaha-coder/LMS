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
    public sealed class AddChapterCommandHandler : ICommandHandler<AddChapterCommand, Result>
    {
        private readonly ICurriculumRepository _curriculumRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AddChapterCommandHandler> _logger;

        public AddChapterCommandHandler(
            ICurriculumRepository curriculumRepository,
            IUnitOfWork unitOfWork,
            ILogger<AddChapterCommandHandler> logger)
        {
            _curriculumRepository = curriculumRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result> Handle(AddChapterCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting to add a new chapter for CurriculumId: {CurriculumId}", request.CurriculumId);

            // الخطوة 1: تحويل البيانات إلى كائنات قيمية
            var titleResult = Title.Create(request.Title);
            var sortOrderResult = SortOrder.Create(request.SortOrder);

            // يمكنك إضافة التحقق من فشل إنشاء الكائنات القيمية هنا
            if (titleResult.IsFailure) return Result.Failure(titleResult.Error);
            if (sortOrderResult.IsFailure) return Result.Failure(sortOrderResult.Error);

            // الخطوة 2: جلب الـ Aggregate Root
            var curriculum = await _curriculumRepository.GetByIdAsync(request.CurriculumId, cancellationToken);

            if (curriculum is null)
            {
                _logger.LogWarning("Curriculum with Id {CurriculumId} was not found.", request.CurriculumId);
                return Result.Failure(CurriculumErrors.NotFound);
            }

            _logger.LogInformation("Found curriculum. Executing domain logic to add chapter...");

            // الخطوة 3: تنفيذ منطق العمل الموجود داخل الـ Domain
            var addChapterResult = curriculum.AddChapter(titleResult.Value, sortOrderResult.Value);

            if (addChapterResult.IsFailure)
            {
                _logger.LogWarning("Failed to add chapter. Reason: {Error}", addChapterResult.Error);
                return addChapterResult;
            }

            // الخطوة 4: حفظ التغييرات
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully added a new chapter for CurriculumId: {CurriculumId}", request.CurriculumId);

            return Result.Success();
        }
    }
}