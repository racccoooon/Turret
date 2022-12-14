// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Turret.Api.Models;

#nullable disable

namespace Turret.Api.Migrations
{
    [DbContext(typeof(TurretDbContext))]
    [Migration("20221111193600_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Turret.Api.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("display_name");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<byte[]>("HashedPassword")
                        .IsRequired()
                        .HasColumnType("bytea")
                        .HasColumnName("hashed_password");

                    b.Property<byte[]>("Salt")
                        .IsRequired()
                        .HasColumnType("bytea")
                        .HasColumnName("salt");

                    b.HasKey("Id")
                        .HasName("pk_user");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasDatabaseName("ix_user_email");

                    b.ToTable("user", null, t =>
                        {
                            t.HasCheckConstraint("CK_user_display_name_Length", "LENGTH(\"display_name\") <= 64");

                            t.HasCheckConstraint("CK_user_email_Length", "LENGTH(\"email\") <= 320");

                            t.HasCheckConstraint("CK_user_email_Regex", "\"email\" ~ '^([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,3})+)$'");
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
