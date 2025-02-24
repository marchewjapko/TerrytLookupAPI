using MediatR;
using TerrytLookup.UseCases.Dtos.Dto.Terryt;

namespace TerrytLookup.UseCases.Queries.FileStreamReaders.GetSimcDtosFromFileStream;

public record GetSimcDtosFromFileStreamQuery(Stream Stream) : IRequest<IList<SimcDto>>;