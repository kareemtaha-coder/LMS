using LMS.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.Shared.ValueObjects
{
    public record Title
    {
        public const int MaxLength = 200;
        public string Value { get; init; }

        // 1. جعل الـ constructor خاصاً لمنع الإنشاء المباشر
        private Title(string value)
        {
            Value = value;
        }

        // 2. توفير Factory Method عام يرجع Result
        public static Result<Title> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Result.Failure<Title>(new Error(
                    "Title.Empty",
                    "Title cannot be empty."));
            }

            if (value.Length > MaxLength)
            {
                return Result.Failure<Title>(new Error(
                    "Title.TooLong",
                    $"Title must not exceed {MaxLength} characters."));
            }

            return new Title(value);
        }
    }
}
