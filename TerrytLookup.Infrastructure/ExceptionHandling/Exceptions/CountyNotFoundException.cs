﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TerrytLookup.Infrastructure.ExceptionHandling.Exceptions;

public class CountyNotFoundException(int voivodeshipId, int countyId) : Exception($"County with id {voivodeshipId}/{countyId} not found."), ICustomMappedException
{
    public ProblemDetails GetProblemDetails(Exception exception)
    {
        return new ProblemDetails
        {
            Title = "County not found",
            Detail = exception.Message,
            Status = StatusCodes.Status404NotFound
        };
    }
}