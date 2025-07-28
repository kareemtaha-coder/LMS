using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Features.LessonContents.AddRichTextContent
{
    public class AddRichTextContentCommandValidator : AbstractValidator<AddRichTextContentCommand>
    {
        public AddRichTextContentCommandValidator()
        {
            RuleFor(x => x.LessonId).NotEmpty();
            RuleFor(x => x.SortOrder).GreaterThan(0);

            // Ensure at least one text field is provided.
            RuleFor(x => x)
                .Must(x => !string.IsNullOrWhiteSpace(x.ArabicText) || !string.IsNullOrWhiteSpace(x.EnglishText))
                .WithMessage("At least one text (Arabic or English) must be provided.");
        }
    }
}
