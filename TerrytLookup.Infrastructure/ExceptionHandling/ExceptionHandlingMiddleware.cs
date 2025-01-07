using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TerrytLookup.Infrastructure.ExceptionHandling;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    private static readonly ProblemDetails DefaultProblemDetails = new()
    {
        Title = "Internal Server Error",
        Detail = "Server has encountered an error",
        Status = StatusCodes.Status500InternalServerError
    };

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        logger.LogError(exception, "An error occurred: {Message}", exception.Message);

        context.Response.ContentType = "application/json";

        if (exception is ICustomMappedException e)
        {
            var problemDetail = e.GetProblemDetails(exception);

            context.Response.StatusCode = problemDetail.Status!.Value;

            return Results.Problem(problemDetail).ExecuteAsync(context);
        }

        context.Response.StatusCode = DefaultProblemDetails.Status!.Value;

        return Results.Problem(DefaultProblemDetails).ExecuteAsync(context);
    }
}