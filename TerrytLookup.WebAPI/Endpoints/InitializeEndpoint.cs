using FastEndpoints;
using MediatR;
using TerrytLookup.Core.Exceptions.CustomExceptions;
using TerrytLookup.UseCases.Commands.InitializeDatabase;
using TerrytLookup.UseCases.Queries.GetNonEmptyRepositories;

namespace TerrytLookup.WebAPI.Endpoints;

/// <summary>
///     Initialize Terryt registries
/// </summary>
/// <remarks>
///     Parse, map and save Terryt data from a CSV files
/// </remarks>
public class InitializeEndpoint(IMediator mediator, ILogger<InitializeEndpoint> logger)
    : Endpoint<InitializeEndpointRequest, EmptyResponse>
{
    /// <summary>
    ///     Configures the endpoint settings
    /// </summary>
    public override void Configure()
    {
        Post("/initialize");
        AllowFileUploads();
        AllowAnonymous();
        Description(b => b.Produces(StatusCodes.Status204NoContent));
    }


    /// <summary>
    ///     Handles the incoming request to initialize the database.
    ///     Validates the file types, checks if the database is empty, and initializes it with the provided files.
    /// </summary>
    /// <param name="request">The request containing the files to be uploaded for initialization.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="InvalidFileContentTypeExtensionException">Thrown when the uploaded file is not a CSV.</exception>
    /// <exception cref="DatabaseNotEmptyException">Thrown when the database is not empty.</exception>
    public override async Task HandleAsync(InitializeEndpointRequest request, CancellationToken cancellationToken)
    {
        IFormFile[] files = [request.TercFile, request.SimcFile, request.UlicFile];
        foreach (var contentType in files.Select(x => x.ContentType))
            if (contentType != "text/csv")
                throw new InvalidFileContentTypeExtensionException(contentType);

        var nonEmptyQuery = new GetNonEmptyRepositoriesQuery();

        var nonEmpty = await mediator.Send(nonEmptyQuery, cancellationToken);

        if (nonEmpty.Count != 0)
            throw new DatabaseNotEmptyException(nonEmpty);

        await mediator.Send(
            new InitializeDatabaseCommand(
                request.TercFile.OpenReadStream(),
                request.SimcFile.OpenReadStream(),
                request.UlicFile.OpenReadStream()),
            cancellationToken);

        logger.Log(LogLevel.Information, "Terryt data initialized.");

        await SendNoContentAsync(cancellationToken);
    }
}

/// <summary>
///     Represents the request for initializing the database with CSV files.
/// </summary>
public class InitializeEndpointRequest
{
    /// <summary>
    ///     TERC file to be uploaded for database initialization.
    /// </summary>
    public required IFormFile TercFile { get; init; }

    /// <summary>
    ///     SIMC file to be uploaded for database initialization.
    /// </summary>
    public required IFormFile SimcFile { get; init; }

    /// <summary>
    ///     ULIC file to be uploaded for database initialization.
    /// </summary>
    public required IFormFile UlicFile { get; init; }
}