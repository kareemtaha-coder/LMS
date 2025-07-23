using LMS.Domain.Abstractions;
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

        // 1. جعل الـ constructor خاصاً
        private Introduction(string value)
        {
            Value = value;
        }

        // 2. توفير Factory Method عام يرجع Result
        public static Result<Introduction> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Result.Failure<Introduction>(new Error(
                    "Introduction.Empty",
                    "Introduction cannot be empty."));
            }

            return new Introduction(value);
        }
    }
}

