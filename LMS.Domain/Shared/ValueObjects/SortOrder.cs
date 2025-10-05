using LMS.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.Shared.ValueObjects
{
    public record SortOrder
    {
        public int Value { get; init; }

        // اجعله خاصاً
        private SortOrder(int value)
        {
            Value = value;
        }

        // أضف Factory Method
        public static Result<SortOrder> Create(int value)
        {
            if (value <= 0)
            {
                return Result.Failure<SortOrder>(new Error(
                    "SortOrder.NotPositive",
                    "Sort order must be a positive number."));
            }
            return new SortOrder(value);
        }
    
}
}
