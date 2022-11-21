using CheckConstraints;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Turret.Api.Models;

public record ProjectId : EntityIdBase<ProjectId>;

public class Project : EntityBase<ProjectId>
{
    public required string Key { get; set; }
    public required string DisplayName { get; set; }
    public string? Description { get; set; }
}

public class ProjectConfiguration : EntityBaseConfiguration<Project, ProjectId>
{
    public const int DisplayNameMaxLength = 64;
    public const int KeyMinLength = 3;
    public const int KeyMaxLength = 8;
    public const int DescriptionMaxLength = 10_000;

    public override void Configure(EntityTypeBuilder<Project> builder)
    {
        base.Configure(builder);

        builder.HasIndex(x => x.Key)
            .IsUnique();

        builder.Property(x => x.Key)
            .HasLengthConstraint(KeyMaxLength, KeyMinLength);

        builder.Property(x => x.DisplayName)
            .HasLengthConstraint(DisplayNameMaxLength);

        builder.Property(x => x.Description)
            .HasNullableLengthConstraint(DescriptionMaxLength);
    }
}