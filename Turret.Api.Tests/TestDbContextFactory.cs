using Microsoft.EntityFrameworkCore;
using Turret.Api.Models;

namespace Turret.Api.Tests;

public class TestDbContextFactory : IDbContextFactory<TurretDbContext>
{
    private readonly TurretDbContext _dbContext;

    public TestDbContextFactory(TurretDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public TurretDbContext CreateDbContext()
    {
        return _dbContext;
    }
}