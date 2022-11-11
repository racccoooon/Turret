namespace Turret.Api.Controllers;

public class RegisterRequestDto
{
    public required string DisplayName { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
}