using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.Abstractions
{

    /// <summary>
    /// Represents a specific validation error that contains multiple granular errors.
    /// يمثل خطأ تحقق محدد يحتوي على عدة أخطاء تفصيلية.
    /// It inherits from 'Error' to be compatible with the 'Result' pattern.
    /// يرث من 'Error' ليكون متوافقًا مع نمط 'Result'.
    /// </summary>
    public sealed class ValidationError : Error
    {
        /// <summary>
        /// A collection of individual property-specific errors.
        /// مجموعة من الأخطاء الفردية الخاصة بكل حقل.
        /// </summary>
        public IReadOnlyCollection<ValidationErrorDetail> Errors { get; }

        public ValidationError(IReadOnlyCollection<ValidationErrorDetail> errors)
            : base("Validation.General", "One or more validation errors occurred.") // كود ورسالة عامة
        {
            Errors = errors;
        }
    }
}
