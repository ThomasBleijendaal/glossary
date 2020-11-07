﻿// <auto-generated />
using EFDapper.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EFDapper.Repositories.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0-rc.1.20451.13");

            modelBuilder.Entity("EFDapper.Repositories.Entities.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("PersonId")
                        .HasColumnType("int");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Zipcode")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("PersonId");

                    b.ToTable("Addresses");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            City = "City 1",
                            Country = "Country 1",
                            PersonId = 1,
                            Street = "Street 123",
                            Zipcode = "ZIP123"
                        },
                        new
                        {
                            Id = 2,
                            City = "City 2",
                            Country = "Country 1",
                            PersonId = 1,
                            Street = "Street 1234",
                            Zipcode = "ZIP1234"
                        });
                });

            modelBuilder.Entity("EFDapper.Repositories.Entities.EmailAddress", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("Emailaddress")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("PersonId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PersonId");

                    b.ToTable("EmailAddresses");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Active = true,
                            Emailaddress = "emailaddress1@example.com",
                            PersonId = 1
                        },
                        new
                        {
                            Id = 2,
                            Active = true,
                            Emailaddress = "emailaddress2@example.com",
                            PersonId = 1
                        },
                        new
                        {
                            Id = 3,
                            Active = true,
                            Emailaddress = "emailaddress3@example.com",
                            PersonId = 1
                        });
                });

            modelBuilder.Entity("EFDapper.Repositories.Entities.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("People");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Test Person 1"
                        });
                });

            modelBuilder.Entity("EFDapper.Repositories.Entities.Address", b =>
                {
                    b.HasOne("EFDapper.Repositories.Entities.Person", "Person")
                        .WithMany("Addresses")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");
                });

            modelBuilder.Entity("EFDapper.Repositories.Entities.EmailAddress", b =>
                {
                    b.HasOne("EFDapper.Repositories.Entities.Person", "Person")
                        .WithMany("EmailAddresses")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");
                });

            modelBuilder.Entity("EFDapper.Repositories.Entities.Person", b =>
                {
                    b.Navigation("Addresses");

                    b.Navigation("EmailAddresses");
                });
#pragma warning restore 612, 618
        }
    }
}