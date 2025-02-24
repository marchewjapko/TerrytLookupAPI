using MediatR;

namespace TerrytLookup.UseCases.Commands.InitializeDatabase;

public record InitializeDatabaseCommand(Stream TercFileStream, Stream SimcFileStream, Stream UlicFileStream) : IRequest;