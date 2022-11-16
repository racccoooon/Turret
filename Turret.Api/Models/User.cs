using Darkarotte.CheckConstraints;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Turret.Api.Models;

public record UserId : EntityIdBase<UserId>;

public class User : EntityBase<UserId>
{
    public required string Email { get; set; }
    public required byte[] HashedPassword { get; set; }
    public required byte[] Salt { get; set; }
    public required string DisplayName { get; set; }
    public List<Session> Sessions { get; set; } = new();
}

public class UserConfiguration : EntityBaseConfiguration<User, UserId>
{
    public const int EmailMaxLength = 320;
    public const int DisplayNameMaxLength = 64;
    public const int UnhashedPasswordMinLength = 8;

    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.HasIndex(x => x.Email)
            .IsUnique();

        builder.Property(x => x.Email)
            .HasLengthConstraint(EmailMaxLength)
            .HasEmailConstraint();

        builder.Property(x => x.DisplayName)
            .HasLengthConstraint(DisplayNameMaxLength);
    }
}