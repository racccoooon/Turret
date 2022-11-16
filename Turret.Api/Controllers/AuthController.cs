using System.Net;
using HttpExceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Turret.Api.Commands;
using Turret.Api.Models;

namespace Turret.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : TurretControllerBase
{
    public AuthController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("user")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register(RegisterRequestDto requestDto, CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand(requestDto.DisplayName, requestDto.Email, requestDto.Password);
        await Mediator.Send(command, cancellationToken);
        return Ok();
    }

    [HttpPost("session")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Login(LoginRequestDto requestDto, CancellationToken cancellationToken)
    {
        var command = new LoginUserCommand(requestDto.Email, requestDto.Password);
        var response = await Mediator.Send(command, cancellationToken);
        HttpContext.Response.Cookies.Append("SessionId", response.SessionId.Value.ToString());
        return Ok();
    }
}