using MediatR;
using TerrytLookup.UseCases.Dtos.Dto.Terryt;

namespace TerrytLookup.UseCases.Commands.AddTowns;

public record AddTownsCommand(IEnumerable<SimcDto> Towns) : IRequest;