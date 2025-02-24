using MediatR;
using TerrytLookup.UseCases.Dtos.Dto;

namespace TerrytLookup.UseCases.Queries.Voivodeships.BrowseVoivodeships;

public record BrowseVoivodeshipsQuery : IStreamRequest<VoivodeshipDto>;