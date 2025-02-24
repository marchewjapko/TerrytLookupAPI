using MediatR;
using TerrytLookup.UseCases.Dtos.Dto.Terryt;

namespace TerrytLookup.UseCases.Commands.AddCounties;

public record AddCountiesCommand(IEnumerable<TercDto> Counties) : IRequest;