using Microsoft.AspNetCore.Http;
using Vendors.Domain.Abstractions;

namespace Vendors.Presentation;

/// <summary>Maps a failed <see cref="Result"/> to an RFC-7807 ProblemDetails response.</summary>
public static class ApiResults
{
    public static IResult Problem(Result result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException("Cannot produce a problem response from a successful result.");
        }

        Error error = result.Error;

        int statusCode = error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        return Results.Problem(
            title: error.Code,
            detail: error.Description,
            statusCode: statusCode);
    }
}
