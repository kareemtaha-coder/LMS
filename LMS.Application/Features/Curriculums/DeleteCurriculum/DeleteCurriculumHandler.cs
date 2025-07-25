using LMS.Application.Abstractions.Messaging;
using LMS.Domain.Abstractions;
using LMS.Domain.Curriculums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.Curriculums.DeleteCurriculum
{
    internal sealed class DeleteCurriculumHandler : ICommandHandler<DeleteCurriculumCommand, Result>
    {
        private readonly ICurriculumRepository _curriculumRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCurriculumHandler(ICurriculumRepository curriculumRepository, IUnitOfWork unitOfWork)
        {
            _curriculumRepository = curriculumRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteCurriculumCommand request, CancellationToken cancellationToken)
        {
            //// 1. العثور على المنهج باستخدام الـ Repository
            //var curriculum = await _curriculumRepository.GetByIdAsync(request.CurriculumId, cancellationToken);

            //if (curriculum is null)
            //{
            //    return Result.Failure(new Error(
            //        "Curriculum.NotFound",
            //        $"The curriculum with ID '{request.CurriculumId}' was not found."));
            //}

            //// 2. طلب الحذف من الـ Repository
            //_curriculumRepository.Delete(curriculum);

            //// 3. حفظ التغييرات في قاعدة البيانات
            //await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}