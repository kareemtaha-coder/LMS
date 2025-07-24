using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.Abstractions
{
    public class Error : IEquatable<Error>
    {
        /// <summary>
        /// A unique code for the error, e.g., "Users.NotFound".
        /// رمز فريد للخطأ.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// A descriptive message for the error.
        /// رسالة وصفية للخطأ.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Represents the absence of an error.
        /// يمثل حالة عدم وجود خطأ.
        /// </summary>
        public static readonly Error None = new(string.Empty, string.Empty);

        public Error(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public bool Equals(Error? other)
        {
            if (other is null)
            {
                return false;
            }
            return Code == other.Code;
        }

        public override bool Equals(object? obj) => obj is Error error && Equals(error);

        public override int GetHashCode() => Code.GetHashCode();
    }
}
