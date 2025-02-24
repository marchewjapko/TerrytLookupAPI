using MediatR;
using TerrytLookup.UseCases.Dtos.Dto.Terryt;

namespace TerrytLookup.UseCases.Queries.FileStreamReaders.GetTercDtosFromFileStream;

public record GetTercDtosFromFileStreamQuery(Stream Stream) : IRequest<IList<TercDto>>;