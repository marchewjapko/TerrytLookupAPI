using MediatR;
using TerrytLookup.UseCases.Dtos.Dto.Terryt;

namespace TerrytLookup.UseCases.Commands.AddVoivodeships;

public record AddVoivodeshipsCommand(IEnumerable<TercDto> Voivodeships) : IRequest;