using MediatR;

namespace TerrytLookup.UseCases.Queries.Voivodeships.ExistAnyVoivodeship;

public record ExistAnyVoivodeshipQuery : IRequest<bool>;