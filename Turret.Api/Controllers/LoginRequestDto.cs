using Microsoft.Extensions.ObjectPool;

namespace Turret.Api.Controllers;

public class LoginRequestDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}