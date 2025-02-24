using MediatR;
using TerrytLookup.Core.Interfaces;
using TerrytLookup.UseCases.Dtos.Dto.Terryt;

namespace TerrytLookup.UseCases.Queries.FileStreamReaders.GetSimcDtosFromFileStream;

public class GetSimcDtosFromFileStreamQueryHandler(IFileStreamReaderService<SimcDto> readerService)
    : IRequestHandler<GetSimcDtosFromFileStreamQuery, IList<SimcDto>>
{
    public Task<IList<SimcDto>> Handle(GetSimcDtosFromFileStreamQuery request, CancellationToken cancellationToken)
    {
        return readerService.ReadCsvFromStream(request.Stream, cancellationToken);
    }
}