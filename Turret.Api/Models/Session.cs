using NodaTime;

namespace Turret.Api.Models;

public record SessionId : EntityIdBase<SessionId>;

public class Session : EntityBase<SessionId>
{
    public required UserId UserId { get; set; }
    public User User { get; set; } = default!;
    public required Instant CreatedAt { get; set; }
    public required Instant LastVisitedAt { get; set; }
}

public class SessionConfiguration : EntityBaseConfiguration<Session, SessionId>
{
}