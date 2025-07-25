using FluentValidation;
using LMS.Domain.Curriculums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.Curriculums.AddChapter
{
   public sealed class AddChapterCommandValidator : AbstractValidator<AddChapterCommand>
{
    public AddChapterCommandValidator()
    {
        RuleFor(x => x.CurriculumId)
            .NotEmpty();

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.SortOrder)
            .GreaterThan(0);
    }

}
}
