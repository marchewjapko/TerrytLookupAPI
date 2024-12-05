using Microsoft.AspNetCore.Mvc;
using TerrytLookup.Infrastructure.Models.Dto;
using TerrytLookup.Infrastructure.Services.StreetService;

namespace TerrytLookup.WebAPI.Controllers;

/// <summary>
///     Controller for managing street-related operations.
/// </summary>
[ApiController]
[Route("[Controller]")]
public class StreetController(IStreetService streetService) : ControllerBase
{
    /// <summary>
    ///     Retrieves a list of all streets, optionally filtered by name and town ID.
    /// </summary>
    /// <param name="name" example="Kostki Potockiego">The optional name of the street to filter by.</param>
    /// <param name="townId" example="988856">The optional ID of the town to filter by.</param>
    /// <returns>A list of <see cref="StreetDto" /> representing the streets.</returns>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<StreetDto>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    [HttpGet]
    public IActionResult BrowseAllStreets(string? name = null, int? townId = null)
    {
        var result = streetService.BrowseAllAsync(name, townId);

        return Ok(result);
    }

    /// <summary>
    ///     Retrieves a specific street by its unique identifier.
    /// </summary>
    /// <param name="townId" example="919810">Identifier of the town the street is located in.</param>
    /// <param name="nameId" example="17284">ID of the street's name.</param>
    /// <returns>The <see cref="StreetDto" /> representing the street.</returns>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StreetDto))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    [HttpGet("{townId:int}/{nameId:int}")]
    public async Task<IActionResult> GetStreetById(int townId, int nameId)
    {
        var result = await streetService.GetByIdAsync(townId, nameId);

        return Ok(result);
    }
}