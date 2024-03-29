﻿// <auto-generated />
using System;
using API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace API.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20211104161237_StudentUpolNumNull")]
    partial class StudentUpolNumNull
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("API.Entities.AppRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("API.Entities.AppUserRole", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("API.Entities.Comment", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("created")
                        .HasColumnType("text");

                    b.Property<string>("studentName")
                        .HasColumnType("text");

                    b.Property<string>("text")
                        .HasColumnType("text");

                    b.Property<int>("topicID")
                        .HasColumnType("integer");

                    b.HasKey("ID");

                    b.HasIndex("topicID");

                    b.ToTable("Comment");
                });

            modelBuilder.Entity("API.Entities.Hodnoceni", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("created")
                        .HasColumnType("text");

                    b.Property<int>("predmetID")
                        .HasColumnType("integer");

                    b.Property<int>("rating")
                        .HasColumnType("integer");

                    b.Property<string>("studentName")
                        .HasColumnType("text");

                    b.Property<string>("text")
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.HasIndex("predmetID");

                    b.ToTable("Hodnoceni");
                });

            modelBuilder.Entity("API.Entities.Predmet", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<int?>("doporucenyRocnik")
                        .HasColumnType("integer");

                    b.Property<string>("doporucenySemestr")
                        .HasColumnType("text");

                    b.Property<string>("katedra")
                        .HasColumnType("text");

                    b.Property<int?>("kreditu")
                        .HasColumnType("integer");

                    b.Property<string>("nazev")
                        .HasColumnType("text");

                    b.Property<int?>("oborIdNum")
                        .HasColumnType("integer");

                    b.Property<string>("rok")
                        .HasColumnType("text");

                    b.Property<string>("rozsah")
                        .HasColumnType("text");

                    b.Property<string>("statut")
                        .HasColumnType("text");

                    b.Property<string>("typZk")
                        .HasColumnType("text");

                    b.Property<string>("vyukaLS")
                        .HasColumnType("text");

                    b.Property<string>("vyukaZS")
                        .HasColumnType("text");

                    b.Property<string>("vyznamPredmetu")
                        .HasColumnType("text");

                    b.Property<string>("zkratka")
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.ToTable("Predmets");
                });

            modelBuilder.Entity("API.Entities.Soubor", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("DateAdded")
                        .HasColumnType("text");

                    b.Property<string>("Extension")
                        .HasColumnType("text");

                    b.Property<string>("FileName")
                        .HasColumnType("text");

                    b.Property<int>("PredmetID")
                        .HasColumnType("integer");

                    b.Property<string>("PublicID")
                        .HasColumnType("text");

                    b.Property<string>("Url")
                        .HasColumnType("text");

                    b.Property<string>("studentName")
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.HasIndex("PredmetID");

                    b.ToTable("Soubor");
                });

            modelBuilder.Entity("API.Entities.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<DateTime>("datumRegistrace")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("oborIdno")
                        .HasColumnType("integer");

                    b.Property<int?>("rocnikRegistrace")
                        .HasColumnType("integer");

                    b.Property<string>("upolNumber")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("API.Entities.Topic", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("created")
                        .HasColumnType("text");

                    b.Property<DateTime>("createdDateTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("name")
                        .HasColumnType("text");

                    b.Property<string>("predmetID")
                        .HasColumnType("text");

                    b.Property<string>("studentName")
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.ToTable("Topics");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("PredmetStudent", b =>
                {
                    b.Property<int>("StudentsId")
                        .HasColumnType("integer");

                    b.Property<int>("predmetyStudentaID")
                        .HasColumnType("integer");

                    b.HasKey("StudentsId", "predmetyStudentaID");

                    b.HasIndex("predmetyStudentaID");

                    b.ToTable("PredmetStudent");
                });

            modelBuilder.Entity("API.Entities.AppUserRole", b =>
                {
                    b.HasOne("API.Entities.AppRole", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Entities.Student", "Student")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("API.Entities.Comment", b =>
                {
                    b.HasOne("API.Entities.Topic", "topic")
                        .WithMany("comments")
                        .HasForeignKey("topicID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("topic");
                });

            modelBuilder.Entity("API.Entities.Hodnoceni", b =>
                {
                    b.HasOne("API.Entities.Predmet", "predmet")
                        .WithMany("Hodnocenis")
                        .HasForeignKey("predmetID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("predmet");
                });

            modelBuilder.Entity("API.Entities.Soubor", b =>
                {
                    b.HasOne("API.Entities.Predmet", "Predmet")
                        .WithMany("Files")
                        .HasForeignKey("PredmetID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Predmet");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("API.Entities.AppRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("API.Entities.Student", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("API.Entities.Student", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("API.Entities.Student", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PredmetStudent", b =>
                {
                    b.HasOne("API.Entities.Student", null)
                        .WithMany()
                        .HasForeignKey("StudentsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Entities.Predmet", null)
                        .WithMany()
                        .HasForeignKey("predmetyStudentaID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("API.Entities.AppRole", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("API.Entities.Predmet", b =>
                {
                    b.Navigation("Files");

                    b.Navigation("Hodnocenis");
                });

            modelBuilder.Entity("API.Entities.Student", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("API.Entities.Topic", b =>
                {
                    b.Navigation("comments");
                });
#pragma warning restore 612, 618
        }
    }
}
