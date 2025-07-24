using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.Curriculums.CreateCurriculum
{
     public class CreateCurriculumCommandValidator : AbstractValidator<CreateCurriculumCommand>
    {
        public CreateCurriculumCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

            RuleFor(x => x.Introduction)
                .NotEmpty().WithMessage("Introduction is required.");
        }
    }
}
