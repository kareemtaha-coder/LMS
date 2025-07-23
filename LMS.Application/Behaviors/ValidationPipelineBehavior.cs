using FluentValidation;
using LMS.Application.Abstractions.Messaging;
using LMS.Domain.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Behaviors
{
    public class ValidationPipelineBehavior<TRequest, TResponse>
         : IPipelineBehavior<TRequest, TResponse>
         where TRequest : ICommand<TResponse>
         where TResponse : Result
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(
         TRequest request,
         RequestHandlerDelegate<TResponse> next,
         CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return await next();
            }

            var validationFailures = _validators
                .Select(validator => validator.Validate(request))
                .SelectMany(validationResult => validationResult.Errors)
                .Where(validationFailure => validationFailure is not null)
                .ToList();
            if (validationFailures.Any())
            {
                // 1. Convert all validation failures into our own Error objects.
                var errors = validationFailures
                    .Select(f => new Error(f.PropertyName, f.ErrorMessage))
                    .ToList();

                // 2. Create the special ValidationError that contains the list of errors.
                var validationError = new ValidationError(errors);

                // 3. Create the failure result, passing our new ValidationError.
                return CreateFailureResult(validationError);
            }
            return await next();
        }

        private static TResponse CreateFailureResult(Error error)
        {
            // Get the generic argument type from the response (e.g., Guid from Result<Guid>)
            var genericArgument = typeof(TResponse).GetGenericArguments()[0];

            // Find the generic method "Failure<T>" on the Result class
            var failureMethod = typeof(Result)
                .GetMethod(nameof(Result.Failure), 1, new[] { typeof(Error) });

            if (failureMethod is null)
            {
                throw new InvalidOperationException("Unable to find generic Failure method on Result class.");
            }

            // Create a specific method, e.g., Failure<Guid>(Error)
            var genericFailureMethod = failureMethod.MakeGenericMethod(genericArgument);

            // Invoke the method and return the result
            return (TResponse)genericFailureMethod.Invoke(null, new object[] { error })!;
        }
    }
}