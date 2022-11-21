using Microsoft.EntityFrameworkCore;
using Npgsql;
using Turret.Api.Models;
using Turret.Api.Tests.Utils;

namespace Turret.Api.Tests;

[Collection("Database")]
public class DbTest : IDisposable
{
    private readonly TurretDbContext _dbContext;
    protected IDbContextFactory<TurretDbContext> DbContextFactory { get; }

    protected DbTest(DbFixture databaseFixture)
    {
        var testDbName = DbUtils.GenerateDbName(GetType().Name);

        using (var templateConnection = new NpgsqlConnection(databaseFixture.Connection))
        {
            templateConnection.Open();

            using (var command =
                   new NpgsqlCommand($"CREATE DATABASE {testDbName} WITH TEMPLATE {databaseFixture.TemplateDbName}",
                       templateConnection))
            {
                command.ExecuteNonQuery();
            }
        }

        var connection = DbUtils.GenerateConnectionString(testDbName);
        
        var optionsBuilder = new DbContextOptionsBuilder<TurretDbContext>();
        Program.ConfigureDbContext(optionsBuilder, connection);

        _dbContext = new TurretDbContext(optionsBuilder.Options);
        DbContextFactory = new TestDbContextFactory(_dbContext);
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
    }
}