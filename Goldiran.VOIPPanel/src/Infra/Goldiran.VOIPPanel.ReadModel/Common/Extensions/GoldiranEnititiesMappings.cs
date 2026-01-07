using Microsoft.EntityFrameworkCore;


namespace Goldiran.VOIPPanel.QueryHandler.Common.Extensions;

public static class GoldiranEnititiesMappings
{
    /// <summary>
    /// </summary>
    public static void AddCustomGoldiranMappings(this ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GoldiranEnititiesMappings).Assembly);
    }
}
