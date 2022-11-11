using System.Data;
using System.Text;
using FluentValidation;
using HttpExceptions;
using Microsoft.EntityFrameworkCore;
using Turret.Api.Models;
using Turret.Api.Services;

namespace Turret.Api.Commands;

public record RegisterUserCommand(
    string DisplayName,
    string Email,
    string Password
    )
    : CommandBase<RegisterUserResponse>;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.DisplayName)
            .NotEmpty()
            .MaximumLength(UserConfiguration.DisplayNameMaxLength);

        RuleFor(x => x.Email)
            .NotEmpty()
            .MaximumLength(UserConfiguration.EmailMaxLength)
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}

public record RegisterUserResponse
{
    public required UserId UserId { get; init; }
}

public class RegisterUserCommandHandler : CommandHandlerBase<RegisterUserCommand, RegisterUserResponse>
{
    private readonly IDbContextFactory<TurretDbContext> _dbContextFactory;
    private readonly ISecurityService _securityService;
    
    public RegisterUserCommandHandler(IDbContextFactory<TurretDbContext> dbContextFactory, ISecurityService securityService)
    {
        _dbContextFactory = dbContextFactory;
        _securityService = securityService;
    }

    public override async Task<RegisterUserResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        var doesEmailAlreadyExist = await dbContext.Set<User>()
            .AnyAsync(x => x.Email == request.Email, cancellationToken);

        if (doesEmailAlreadyExist)
            throw new HttpConflictException("Account with email already exists");

        var salt = _securityService.GenerateSalt();
        var passwordBytes = Encoding.UTF8.GetBytes(request.Password);
        var hashedPassword = _securityService.HashPassword(passwordBytes, salt);

        var user = new User
        {
            DisplayName = request.DisplayName,
            Email = request.Email,
            HashedPassword = hashedPassword,
            Salt = salt,
        };

        await dbContext.AddAsync(user, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var response = new RegisterUserResponse
        {
            UserId = user.Id,
        };

        return response;
    }
}