using MediatR;
using TerrytLookup.UseCases.Dtos.Dto.Terryt;

namespace TerrytLookup.UseCases.Queries.FileStreamReaders.GetUlicDtosFromFileStream;

public record GetUlicDtosFromFileStreamQuery(Stream Stream) : IRequest<IList<UlicDto>>;