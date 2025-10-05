using LMS.Domain.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiControllerBase : ControllerBase
    {
        protected readonly ISender Sender;

        protected ApiControllerBase(ISender sender)
        {
            Sender = sender;
        }

        protected IActionResult HandleFailure(Result result)
        {
            if (result.IsSuccess)
            {
                throw new InvalidOperationException("A success result cannot be handled as a failure.");
            }

            // Check if the error is a validation error.
            if (result.Error is ValidationError validationError)
            {
                // Create a new ModelStateDictionary to hold the errors.
                var modelState = new ModelStateDictionary();

                // Add each error from our custom ValidationError to the ModelStateDictionary.
                foreach (var error in validationError.Errors)
                {
                    modelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                // Return a 400 Bad Request with a standard ValidationProblemDetails payload.
                return ValidationProblem(modelState);
            }

            // For all other domain errors, return a generic 400 Bad Request.
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Bad Request",
                detail: result.Error.Message
                );
        }
    }
}
