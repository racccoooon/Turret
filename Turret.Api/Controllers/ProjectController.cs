using HttpExceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Turret.Api.Commands;

namespace Turret.Api.Controllers;


[ApiController]
[Route("api/projects")]
public class ProjectController : TurretControllerBase
{
    public ProjectController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(CreateProjectRequestDto requestDto)
    {
        var command = new CreateProjectCommand(requestDto.Key, requestDto.DisplayName);
        var response = await Mediator.Send(command);
        var responseDto = new CreateProjectResponseDto
        {
            ProjectId = response.ProjectId,
        };
        
        return Ok(responseDto);
    }
}