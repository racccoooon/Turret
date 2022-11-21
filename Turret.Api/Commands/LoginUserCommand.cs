using System.Text;
using FluentValidation;
using HttpExceptions;
using Microsoft.EntityFrameworkCore;
using Turret.Api.Models;
using Turret.Api.Services;
using SystemClock = NodaTime.SystemClock;

namespace Turret.Api.Commands;

public record LoginUserCommand(
    string Email,
    string Password
    ) 
    : CommandBase<LoginUserResponse>;

public class LoginUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .MaximumLength(UserConfiguration.EmailMaxLength)
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}

public record LoginUserResponse
{
    public required SessionId SessionId { get; set; }
}

public class LoginUserCommandHandler : CommandHandlerBase<LoginUserCommand, LoginUserResponse>
{
    private readonly IDbContextFactory<TurretDbContext> _dbContextFactory;
    private readonly ISecurityService _securityService;
    
    public LoginUserCommandHandler(IDbContextFactory<TurretDbContext> dbContextFactory, ISecurityService securityService)
    {
        _dbContextFactory = dbContextFactory;
        _securityService = securityService;
    }

    public override async Task<LoginUserResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        // query user
        var userInfo = await dbContext.Set<User>()
            .Where(x => x.Email == request.Email)
            .Select(x => new
            {
                x.Id,
                x.Salt,
                x.HashedPassword,
            })
            .FirstOrDefaultAsync(cancellationToken);
        
        // check if user exists
        if (userInfo == null)
            throw new HttpUnauthorizedException();
        
        // hash request password
        var requestPasswordBytes = Encoding.UTF8.GetBytes(request.Password);
        var hashedRequestPassword = _securityService.HashPassword(requestPasswordBytes, userInfo.Salt);

        // check if user password equals hashed request password
        if (!hashedRequestPassword.SequenceEqual(userInfo.HashedPassword))
            throw new HttpUnauthorizedException();

        // create new session
        var now = SystemClock.Instance.GetCurrentInstant();
        var session = new Session
        {
            UserId = userInfo.Id,
            CreatedAt = now,
            LastVisitedAt = now,
        };

        await dbContext.AddAsync(session, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        // return response
        var response = new LoginUserResponse
        {
            SessionId = session.Id,
        };

        return response;
    }
}
