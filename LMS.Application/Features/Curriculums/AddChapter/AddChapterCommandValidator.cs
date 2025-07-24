using FluentValidation;
using LMS.Domain.Curriculums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.Curriculums.AddChapter
{
    public class AddChapterCommandValidator : AbstractValidator<AddChapterCommand>
    {
       public AddChapterCommandValidator(ICurriculumRepository curriculumRepository)
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.CurriculumId)
            .NotEmpty()
            .WithMessage("Curriculum ID must not be empty.")
            // قاعدة تحقق متقدمة: نتأكد من أن المنهج موجود فعلاً في قاعدة البيانات
            .MustAsync(async (curriculumId, cancellationToken) => 
            {
                return await curriculumRepository.AnyAsync(curriculumId, cancellationToken);
            })
            .WithMessage("The specified curriculum does not exist.");
    }
    }
}
