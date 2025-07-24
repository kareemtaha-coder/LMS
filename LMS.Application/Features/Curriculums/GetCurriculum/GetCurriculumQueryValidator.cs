using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.Curriculums.GetCurriculum
{
    public sealed class GetCurriculumQueryValidator : AbstractValidator<GetCurriculumQuery>
    {
        public GetCurriculumQueryValidator()
        {
            RuleFor(x => x.CurriculumId)
                .NotEmpty().WithMessage("Curriculum ID is required.");
        }
    }
}
