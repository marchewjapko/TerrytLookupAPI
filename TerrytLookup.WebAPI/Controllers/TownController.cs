using Microsoft.AspNetCore.Mvc;
using TerrytLookup.Infrastructure.Models.Dto;
using TerrytLookup.Infrastructure.Services.TownService;

namespace TerrytLookup.WebAPI.Controllers;

/// <summary>
///     Controller for managing town-related operations.
/// </summary>
[ApiController]
[Route("[Controller]")]
public class TownController(ITownService townService) : ControllerBase
{
    /// <summary>
    ///     Retrieves a list of all towns, optionally filtered by name and town ID.
    /// </summary>
    /// <param name="name" example="Warszawa">The optional name of the town to filter by.</param>
    /// <param name="voivodeshipId" example="14">The optional ID of the voivodeship to filter by.</param>
    /// <param name="countyId" example="65">The optional ID of the county to filter by.</param>
    /// <returns>A list of <see cref="TownDto" /> representing the towns.</returns>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TownDto>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    [HttpGet]
    public IActionResult BrowseAllTowns(string? name = null, int? voivodeshipId = null, int? countyId = null)
    {
        var result = townService.BrowseAllAsync(name, voivodeshipId, countyId);

        return Ok(result);
    }

    /// <summary>
    ///     Retrieves a specific town by its unique identifier.
    /// </summary>
    /// <param name="id" example="918123">The unique identifier of the town.</param>
    /// <returns>The <see cref="TownDto" /> representing the town.</returns>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TownDto))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetTownById(int id)
    {
        var result = await townService.GetByIdAsync(id);

        return Ok(result);
    }
}