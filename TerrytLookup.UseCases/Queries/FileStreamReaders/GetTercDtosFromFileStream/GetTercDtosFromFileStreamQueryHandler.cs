using MediatR;
using TerrytLookup.Core.Interfaces;
using TerrytLookup.UseCases.Dtos.Dto.Terryt;

namespace TerrytLookup.UseCases.Queries.FileStreamReaders.GetTercDtosFromFileStream;

public class GetTercDtosFromFileStreamQueryHandler(IFileStreamReaderService<TercDto> readerService)
    : IRequestHandler<GetTercDtosFromFileStreamQuery, IList<TercDto>>
{
    public Task<IList<TercDto>> Handle(GetTercDtosFromFileStreamQuery request, CancellationToken cancellationToken)
    {
        return readerService.ReadCsvFromStream(request.Stream, cancellationToken);
    }
}