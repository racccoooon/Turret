using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Turret.Api.Controllers;

public class TurretControllerBase : ControllerBase
{
    protected readonly IMediator Mediator;

    public TurretControllerBase(IMediator mediator)
    {
        Mediator = mediator;
    }
}