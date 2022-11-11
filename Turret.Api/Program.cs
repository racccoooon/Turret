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

class Program
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

        builder.Services.AddDbContextFactory<TurretDbContext>(optionsBuilder =>
        {
            optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("Default"),
                    contextOptionsBuilder => contextOptionsBuilder.UseNodaTime())
                .UseSnakeCaseNamingConvention()
                .UseCheckConstraints();
        });

        var app = builder.Build();

        await MigrateDatabase(app);

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

    public static async Task MigrateDatabase(WebApplication app)
    {
        var dbContextFactory = app.Services.GetRequiredService<IDbContextFactory<TurretDbContext>>();
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        await dbContext.Database.MigrateAsync();
    }
}