using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TerrytLookup.Infrastructure.ExceptionHandling.Exceptions;

public class InvalidDatabaseConfigurationException(string error) : Exception(error), ICustomMappedException
{
    public ProblemDetails GetProblemDetails(Exception exception)
    {
        return new ProblemDetails()
        {
            Title = "Invalid database configuration.",
            Detail = exception.Message,
            Status = StatusCodes.Status500InternalServerError,
        };
    }
}