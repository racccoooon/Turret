using Turret.Api.Models;

namespace Turret.Api.Controllers;

public class CreateProjectRequestDto
{
    public required string Key { get; init; }
    public required string DisplayName { get; init; }
}

public class CreateProjectResponseDto
{
    public required ProjectId ProjectId { get; init; }
}