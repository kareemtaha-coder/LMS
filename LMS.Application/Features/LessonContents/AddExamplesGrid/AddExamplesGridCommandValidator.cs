using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.LessonContents.AddExamplesGrid
{

    public class AddExamplesGridCommandValidator : AbstractValidator<AddExamplesGridCommand>
    {
        public AddExamplesGridCommandValidator()
        {
            RuleFor(x => x.LessonId).NotEmpty();
            RuleFor(x => x.SortOrder).GreaterThan(0);
        }
    }
}
