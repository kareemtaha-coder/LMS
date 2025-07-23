using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.Curriculums
{
    public record Introduction
    {
        public string Value { get; init; }
        public Introduction(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
            throw new ArgumentException("Introduction Cant Be Empty");
            }
                Value = value;

        }
    }
}
