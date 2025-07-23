using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.Shared.ValueObjects
{
    public record SortOrder
    {
        public int Value{ get; init; }
        public SortOrder(int value)
        {
            if (value <= 0)
            {
                throw new ArgumentException("Sort order must be a positive number.", nameof(value));
            }
            Value = value;

        }
    }
}
