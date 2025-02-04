﻿// <auto-generated />
using System;
using Example.Web.Domains.EfCore.Persistence.Sql;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Example.Web.Domains.EfCore.Persistence.Sql.Migrations
{
    [DbContext(typeof(EfCoreDbContext))]
    partial class EfCoreDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("EfCore")
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("Example.Web.Domains.EfCore.Domain.Entities.TestEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("TestEntities", "EfCore");

                    b.HasData(
                        new
                        {
                            Id = new Guid("f5daebb9-a4b3-4e75-81ab-6607c770316a"),
                            CreatedAt = new DateTime(2025, 2, 1, 17, 1, 8, 685, DateTimeKind.Utc).AddTicks(6061),
                            FirstName = "Test",
                            LastName = "Test"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
