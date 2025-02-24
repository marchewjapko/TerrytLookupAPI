using TerrytLookup.Core.Domain;
using TerrytLookup.Core.Repositories;
using TerrytLookup.Infrastructure.Repositories.DbContext;

namespace TerrytLookup.Infrastructure.Repositories;

public class TownRepository(AppDbContext dbContext) : BaseRepository<Town>(dbContext), ITownRepository;