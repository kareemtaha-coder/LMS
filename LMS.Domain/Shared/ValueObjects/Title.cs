using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.Shared.ValueObjects
{
    public record Title
    {
        public string Value { get; init; }
        public Title(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
            throw new ArgumentException("Title Can't Be Empty ",nameof(value));
            }
                Value = value;
        }
    }
}
