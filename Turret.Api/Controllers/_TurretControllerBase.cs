using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Turret.Api.Controllers;

public class TurretControllerBase : ControllerBase
{
    protected readonly IMediator Mediator;

    public TurretControllerBase(IMediator mediator)
    {
        Mediator = mediator;
    }
}