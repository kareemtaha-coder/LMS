using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.Abstractions
{
    public sealed record ValidationError(IReadOnlyCollection<Error> Errors)
     : Error("Validation.Failure", "One or more validation errors occurred.");
}
