﻿using CsvHelper.Configuration.Attributes;

namespace TerrytLookup.Infrastructure.Models.Dto.Terryt;

public sealed class UlicDto
{
    /// <summary>
    ///     Terryt property: <c>WOJ</c>
    /// </summary>
    [Name("WOJ")]
    public required int VoivodeshipId { get; init; }

    /// <summary>
    ///     Terryt property: <c>POW</c>
    /// </summary>
    [Name("POW")]
    public required int CountyId { get; init; }

    /// <summary>
    ///     Terryt property: <c>GMI</c>
    /// </summary>
    [Name("GMI")]
    public required int MunicipalityId { get; init; }

    /// <summary>
    ///     Terryt property: <c>RODZ_GMI</c>
    /// </summary>
    [Name("RODZ_GMI")]
    public required int EntityTypeId { get; init; }

    /// <summary>
    ///     Terryt property: <c>SYM</c>
    /// </summary>
    [Name("SYM")]
    public required int TownId { get; init; }

    /// <summary>
    ///     Terryt property: <c>SYM_UL</c>
    /// </summary>
    /// <remarks>
    ///     This property is used to identify a street's name, not the street itself.
    ///     Thusly it is not unique
    /// </remarks>
    [Name("SYM_UL")]
    public required int StreetNameId { get; init; }

    /// <summary>
    ///     Terryt property: <c>CECHA</c>
    /// </summary>
    [Name("CECHA")]
    public required string StreetPrefix { get; init; }

    /// <summary>
    ///     Terryt property: <c>NAZWA_1</c>
    /// </summary>
    [Name("NAZWA_1")]
    public required string StreetNameFirstPart { get; init; }

    /// <summary>
    ///     Terryt property: <c>NAZWA_2</c>
    /// </summary>
    [Name("NAZWA_2")]
    public string? StreetNameSecondPart { get; init; }

    /// <summary>
    ///     Terryt property: <c>STAN_NA</c>
    /// </summary>
    [Name("STAN_NA")]
    public required DateOnly ValidFromDate { get; init; }
}