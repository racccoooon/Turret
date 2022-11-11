using FluentValidation;
using HttpExceptions;
using Microsoft.EntityFrameworkCore;
using Turret.Api.Models;

namespace Turret.Api.Commands;

public record CreateProjectCommand(
    string Key,
    string DisplayName
    )
    : CommandBase<CreateProjectResponse>;

public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(x => x.Key)
            .NotEmpty()
            .MaximumLength(ProjectConfiguration.KeyMaxLength)
            .MinimumLength(ProjectConfiguration.KeyMinLength);

        RuleFor(x => x.DisplayName)
            .NotEmpty()
            .MaximumLength(ProjectConfiguration.DisplayNameMaxLength);
    }
}

public record CreateProjectResponse
{
    public required ProjectId ProjectId { get; init; }
}

public class CreateProjectCommandHandler : CommandHandlerBase<CreateProjectCommand, CreateProjectResponse>
{
    private readonly IDbContextFactory<TurretDbContext> _dbContextFactory;

    public CreateProjectCommandHandler(IDbContextFactory<TurretDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public override async Task<CreateProjectResponse> Handle(CreateProjectCommand request, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        var doesKeyAlreadyExist = await dbContext.Set<Project>()
            .AnyAsync(x => x.Key == request.Key, cancellationToken);

        if (doesKeyAlreadyExist)
            throw new HttpConflictException("Project with key already exists");

        var project = new Project
        {
            Key = request.Key,
            DisplayName = request.DisplayName,
        };

        await dbContext.AddAsync(project, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var response = new CreateProjectResponse
        {
            ProjectId = project.Id,
        };

        return response;
    }
}