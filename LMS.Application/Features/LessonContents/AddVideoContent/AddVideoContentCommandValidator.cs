using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.LessonContents.AddVideoContent
{
    public class AddVideoContentCommandValidator : AbstractValidator<AddVideoContentCommand>
    {
        public AddVideoContentCommandValidator()
        {
            RuleFor(x => x.LessonId).NotEmpty();
            RuleFor(x => x.SortOrder).GreaterThan(0);

            RuleFor(x => x.VideoUrl)
                .NotEmpty()
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
                .WithMessage("A valid URL is required.");
        }
    }
}
