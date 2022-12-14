using Microsoft.EntityFrameworkCore;

namespace Turret.Api.Models;

public class TurretDbContext : DbContext
{
    public TurretDbContext(DbContextOptions<TurretDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TurretDbContext).Assembly);
    }
}