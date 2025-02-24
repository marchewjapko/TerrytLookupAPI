using MediatR;
using TerrytLookup.Core.Interfaces;
using TerrytLookup.UseCases.Dtos.Dto.Terryt;

namespace TerrytLookup.UseCases.Queries.FileStreamReaders.GetUlicDtosFromFileStream;

public class GetUlicDtosFromFileStreamQueryHandler(IFileStreamReaderService<UlicDto> readerService)
    : IRequestHandler<GetUlicDtosFromFileStreamQuery, IList<UlicDto>>
{
    public Task<IList<UlicDto>> Handle(GetUlicDtosFromFileStreamQuery request, CancellationToken cancellationToken)
    {
        return readerService.ReadCsvFromStream(request.Stream, cancellationToken);
    }
}