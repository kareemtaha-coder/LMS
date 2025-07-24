using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.Abstractions
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error Error { get; }

        protected Result(bool isSuccess, Error error)
        {
            // Ensures that a success result never has an error, and a failure result always has an error.
            // يضمن أن النتيجة الناجحة لا تحتوي على خطأ، والنتيجة الفاشلة يجب أن تحتوي على خطأ.
            if (isSuccess && error != Error.None ||
                !isSuccess && error == Error.None)
            {
                throw new InvalidOperationException("Invalid result state.");
            }

            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() => new(true, Error.None);
        public static Result Failure(Error error) => new(false, error);

        // Generic version factory methods
        public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);
        public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);
    }


    public class Result<TValue> : Result
    {
        private readonly TValue? _value;

        public TValue Value => IsSuccess
            ? _value!
            : throw new InvalidOperationException("The value of a failure result can not be accessed.");

        protected internal Result(TValue? value, bool isSuccess, Error error)
            : base(isSuccess, error)
        {
            _value = value;
        }

        // Implicit conversion from TValue to Result<TValue> for convenience.
        // تحويل ضمني من القيمة إلى النتيجة لتسهيل الكود.
        public static implicit operator Result<TValue>(TValue value) => Success(value);

        // Implicit conversion from Error to Result<TValue> for convenience.
        // تحويل ضمني من الخطأ إلى النتيجة لتسهيل الكود.
        public static implicit operator Result<TValue>(Error error) => Failure<TValue>(error);
    }

}
