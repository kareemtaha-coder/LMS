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
        public string Value { get; init; }

        // نجعل الـ constructor خاصًا لمنع الإنشاء المباشر
        private Title(string value)
        {
            Value = value;
        }

        // نستخدم الـ Factory Method مع الـ Result Pattern
        public static Result<Title> Create(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Result.Failure<Title>(new Error(
                    "Title.Empty",
                    "Title cannot be empty."));
            }

            if (value.Length > 100)
            {
                return Result.Failure<Title>(new Error(
                   "Title.TooLong",
                   "Title cannot be longer than 100 characters."));
            }

            return new Title(value);
        }
    }
}
