using FluentValidation;
using LMS.Application.Abstractions.Messaging;
using LMS.Domain.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LMS.Application.Behaviors;

/// <summary>
/// A MediatR pipeline behavior that intercepts commands and performs validation
/// before they reach their handlers.
/// </summary>
public sealed class ValidationPipelineBehavior<TRequest, TResponse>
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
        // If there are no validators, immediately proceed to the handler.
        if (!_validators.Any())
        {
            return await next();
        }

        // Concurrently execute all validators and collect the validation failures.
        var validationFailures = (await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(request, cancellationToken))))
            .SelectMany(validationResult => validationResult.Errors)
            .Where(failure => failure is not null)
            .ToList();

        // If any failures are found, short-circuit the pipeline and return a failure result.
        if (validationFailures.Any())
        {
            // Convert the list of validation failures into our custom domain Error objects.
            var errors = validationFailures
                .Select(f => new Error(f.PropertyName, f.ErrorMessage))
                .ToList();

            // Create a special validation error that encapsulates all the individual errors.
            //var validationError = new ValidationError(errors);
            var validationError = new ValidationError(
                  validationFailures.Select(failure =>
                      new ValidationErrorDetail(failure.PropertyName, failure.ErrorMessage))
                  .ToList());


            // Use the robust helper method to create the appropriate failure Result.
            return CreateFailureResult(validationError);
        }

        // If validation succeeds, proceed to the actual request handler.
        return await next();
    }

    /// <summary>
    /// Creates a failure result of the correct generic type using reflection.
    /// This is a robust implementation that handles both non-generic and generic Result types.
    /// </summary>
    private static TResponse CreateFailureResult(Error error)
    {
        // Handle the simple, non-generic Result case first.
        if (typeof(TResponse) == typeof(Result))
        {
            return (Result.Failure(error) as TResponse)!;
        }

        // Handle the generic Result<T> case.
        var resultType = typeof(TResponse);
        var genericArgument = resultType.GetGenericArguments()[0];

        // Find the static, generic "Failure" method on the base Result class.
        var failureMethod = typeof(Result)
            .GetMethod(nameof(Result.Failure), 1, new[] { typeof(Error) });

        if (failureMethod is null)
        {
            // This should be unreachable if TResponse is a valid Result<T>.
            throw new InvalidOperationException("Could not find the generic Failure method on the Result class.");
        }

        // Create a specific version of the method, e.g., Result.Failure<Guid>(error).
        var genericFailureMethod = failureMethod.MakeGenericMethod(genericArgument);

        // Invoke the static method and cast the result to the expected response type.
        return (TResponse)genericFailureMethod.Invoke(null, new object[] { error })!;
    }
}