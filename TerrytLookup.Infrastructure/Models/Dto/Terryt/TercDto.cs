﻿using CsvHelper.Configuration.Attributes;

namespace TerrytLookup.Infrastructure.Models.Dto.Terryt;

public sealed class TercDto
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
    public int? CountyId { get; init; }

    /// <summary>
    ///     Terryt property: <c>NAZWA</c>
    /// </summary>
    [Name("NAZWA")]
    public required string Name { get; init; }

    /// <summary>
    ///     Terryt property: <c>NAZWA_DOD</c>
    /// </summary>
    [Name("NAZWA_DOD")]
    public required string EntityType { get; init; }

    /// <summary>
    ///     Terryt property: <c>STAN_NA</c>
    /// </summary>
    [Name("STAN_NA")]
    public required DateOnly ValidFromDate { get; init; }

    public bool IsVoivodeship()
    {
        const string voivodeship = "województwo";

        return string.Equals(EntityType, voivodeship, StringComparison.InvariantCultureIgnoreCase);
    }

    public bool IsCounty()
    {
        string[] county = ["powiat", "miasto na prawach powiatu", "miasto stołeczne, na prawach powiatu"];

        return Array.Exists(county,
            x => CountyId.HasValue && string.Equals(EntityType, x, StringComparison.InvariantCultureIgnoreCase));
    }
}