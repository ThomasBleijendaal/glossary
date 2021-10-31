﻿// <auto-generated />
using System;
using EFCoreQueries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EFCoreQueries.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20211004173920_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0-rc.1.20451.13");

            modelBuilder.Entity("EFDapper.Repositories.Entities.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Companies");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Company 1"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Company 2"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Company 3"
                        });
                });

            modelBuilder.Entity("EFDapper.Repositories.Entities.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int?>("CompanyId")
                        .HasColumnType("int");

                    b.Property<int?>("ManagerId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("ManagerId");

                    b.ToTable("Employees");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CompanyId = 1,
                            Name = "Emp 1"
                        },
                        new
                        {
                            Id = 2,
                            CompanyId = 1,
                            ManagerId = 1,
                            Name = "Emp 2"
                        },
                        new
                        {
                            Id = 3,
                            CompanyId = 1,
                            ManagerId = 1,
                            Name = "Emp 3"
                        },
                        new
                        {
                            Id = 4,
                            CompanyId = 2,
                            Name = "Emp 4"
                        },
                        new
                        {
                            Id = 5,
                            CompanyId = 3,
                            ManagerId = 4,
                            Name = "Emp 5"
                        });
                });

            modelBuilder.Entity("EFDapper.Repositories.Entities.Employee", b =>
                {
                    b.HasOne("EFDapper.Repositories.Entities.Company", "Company")
                        .WithMany("Employees")
                        .HasForeignKey("CompanyId");

                    b.HasOne("EFDapper.Repositories.Entities.Employee", "Manager")
                        .WithMany("Minions")
                        .HasForeignKey("ManagerId");

                    b.Navigation("Company");

                    b.Navigation("Manager");
                });

            modelBuilder.Entity("EFDapper.Repositories.Entities.Company", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("EFDapper.Repositories.Entities.Employee", b =>
                {
                    b.Navigation("Minions");
                });
#pragma warning restore 612, 618
        }
    }
}