using Microsoft.EntityFrameworkCore;
using Turret.Api.Models;
using Turret.Api.Tests.Utils;

namespace Turret.Api.Tests;

public class DbFixture : IDisposable
{
    private readonly DbContext _context;

    public string TemplateDbName { get; }
    public string Connection { get; }

    public DbFixture()
    {
        TemplateDbName = DbUtils.GenerateDbName("db_template");
        Connection = DbUtils.GenerateConnectionString(TemplateDbName);

        var optionsBuilder = new DbContextOptionsBuilder<TurretDbContext>();
        Program.ConfigureDbContext(optionsBuilder, Connection);

        _context = new TurretDbContext(optionsBuilder.Options);
        
        _context.Database.EnsureCreated();
        _context.Database.CloseConnection();
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
    }
}