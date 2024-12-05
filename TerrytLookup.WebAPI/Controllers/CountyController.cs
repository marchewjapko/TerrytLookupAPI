﻿using Microsoft.AspNetCore.Mvc;
using TerrytLookup.Infrastructure.Models.Dto;
using TerrytLookup.Infrastructure.Services.CountyService;

namespace TerrytLookup.WebAPI.Controllers;

/// <summary>
///     Controller for managing county-related operations.
/// </summary>
[ApiController]
[Route("[Controller]")]
public class CountyController(ICountyService countyService) : ControllerBase
{
    /// <summary>
    ///     Retrieves a list of all streets, optionally filtered by name and county ID.
    /// </summary>
    /// <param name="name" example="Warszawa">The optional name of the street to filter by.</param>
    /// <param name="voivodeshipId" example="14">The optional ID of the county to filter by.</param>
    /// <returns>A list of <see cref="CountyDto" /> representing the streets.</returns>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CountyDto>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    [HttpGet]
    public IActionResult BrowseAllCounties(string? name = null, int? voivodeshipId = null)
    {
        var result = countyService.BrowseAllAsync(name, voivodeshipId);

        return Ok(result);
    }

    /// <summary>
    ///     Retrieves a specific street by its unique identifier.
    /// </summary>
    /// <param name="voivodeshipId" example="14">The unique identifier of the voivodeship.</param>
    /// <param name="countyId" example="65">The unique identifier of the county within <paramref name="voivodeshipId"/>.</param>
    /// <returns>The <see cref="CountyDto" /> representing the street.</returns>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CountyDto))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    [HttpGet("{voivodeshipId:int}/{countyId:int}")]
    public async Task<IActionResult> GetCountyById(int voivodeshipId, int countyId)
    {
        var result = await countyService.GetByIdAsync(voivodeshipId, countyId);
        
        return Ok(result);
    }
}