using MediatR;
using TerrytLookup.UseCases.Commands.AddCounties;
using TerrytLookup.UseCases.Commands.AddStreets;
using TerrytLookup.UseCases.Commands.AddTowns;
using TerrytLookup.UseCases.Commands.AddVoivodeships;
using TerrytLookup.UseCases.Queries.FileStreamReaders.GetSimcDtosFromFileStream;
using TerrytLookup.UseCases.Queries.FileStreamReaders.GetTercDtosFromFileStream;
using TerrytLookup.UseCases.Queries.FileStreamReaders.GetUlicDtosFromFileStream;

namespace TerrytLookup.UseCases.Commands.InitializeDatabase;

public class InitializeDatabaseCommandHandler(IMediator mediator) : IRequestHandler<InitializeDatabaseCommand>
{
    public async Task Handle(InitializeDatabaseCommand request, CancellationToken cancellationToken)
    {
        var tercDtos = await mediator.Send(
            new GetTercDtosFromFileStreamQuery(request.TercFileStream),
            cancellationToken);

        if (tercDtos.Any())
        {
            await mediator.Send(new AddVoivodeshipsCommand(tercDtos.Where(x => x.IsVoivodeship())), cancellationToken);
            await mediator.Send(new AddCountiesCommand(tercDtos.Where(x => x.IsCounty())), cancellationToken);
        }

        var simcDtos = await mediator.Send(
            new GetSimcDtosFromFileStreamQuery(request.SimcFileStream),
            cancellationToken);

        if (simcDtos.Any())
            await mediator.Send(new AddTownsCommand(simcDtos), cancellationToken);

        var ulicDtos = await mediator.Send(
            new GetUlicDtosFromFileStreamQuery(request.UlicFileStream),
            cancellationToken);

        if (ulicDtos.Any())
            await mediator.Send(new AddStreetsCommand(ulicDtos), cancellationToken);
    }
}