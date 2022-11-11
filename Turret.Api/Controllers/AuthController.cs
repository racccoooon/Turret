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

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register(RegisterRequestDto requestDto)
    {
        var command = new RegisterUserCommand(requestDto.DisplayName, requestDto.Email, requestDto.Password);
        await Mediator.Send(command);
        return Ok();
    }
}