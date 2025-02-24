using MediatR;
using TerrytLookup.UseCases.Dtos.Dto.Terryt;

namespace TerrytLookup.UseCases.Commands.AddStreets;

public record AddStreetsCommand(IEnumerable<UlicDto> Streets) : IRequest;