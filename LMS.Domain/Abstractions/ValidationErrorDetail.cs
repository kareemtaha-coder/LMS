using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.Abstractions
{
    public record ValidationErrorDetail(string PropertyName, string ErrorMessage);
}
