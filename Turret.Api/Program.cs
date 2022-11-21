using System.Collections.Immutable;
using System.Text.Json;
using System.Text.Json.Serialization;
using Darkarotte.CheckConstraints;
using HttpExceptions;
using MediatR;
using MediatR.Extensions.FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Turret.Api.Models;
using Turret.Api.Options;
using Turret.Api.Services;
using Turret.Api.Utils;

namespace Turret.Api;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        // Add services to the container.

        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;

                var enumConverter = new JsonStringEnumConverter(JsonNamingPolicy.CamelCase);
                options.JsonSerializerOptions.Converters.Add(enumConverter);
                
                options.JsonSerializerOptions.AddEntityIdJsonConverterFromAssembly(typeof(Program).Assembly);
            });
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddLazyCache();

        builder.Services.AddMediatR(typeof(Program).Assembly);
        builder.Services.AddFluentValidation(typeof(Program).Assembly.Yield());

        builder.Services.AddOptions<TurretOptions>()
            .Bind(builder.Configuration.GetSection("Turret"))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        builder.Services.AddTransient<ISecurityService, SecurityService>();

        builder.Services.AddTurretDbContext(builder.Configuration.GetConnectionString("Default"));

        var app = builder.Build();

        await app.MigrateDatabase();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpExceptions();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        await app.RunAsync();
    }

    private static async Task MigrateDatabase(this IHost app)
    {
        var dbContextFactory = app.Services.GetRequiredService<IDbContextFactory<TurretDbContext>>();
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        await dbContext.Database.MigrateAsync();
    }

    private static void AddTurretDbContext(this IServiceCollection serviceCollection, string? connectionString)
    {
        serviceCollection.AddDbContextFactory<TurretDbContext>(optionsBuilder =>
        {
            ConfigureDbContext(optionsBuilder, connectionString);
        });
    }

    public static void ConfigureDbContext(DbContextOptionsBuilder optionsBuilder, string? connectionString)
    {
        optionsBuilder.UseNpgsql(connectionString,
                contextOptionsBuilder => contextOptionsBuilder.UseNodaTime())
            .UseSnakeCaseNamingConvention()
            .UseCheckConstraints();
    }
}