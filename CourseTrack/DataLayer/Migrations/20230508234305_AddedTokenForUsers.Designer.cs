﻿// <auto-generated />
using System;
using DataLayer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataLayer.Migrations
{
    [DbContext(typeof(CourseTrackDbContext))]
    [Migration("20230508234305_AddedTokenForUsers")]
    partial class AddedTokenForUsers
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DataLayer.Entities.CourseWorkEntity.CourseWork", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<int?>("LecturerId")
                        .HasColumnType("integer");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("StudentId")
                        .HasColumnType("integer");

                    b.Property<string>("Theme")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.HasIndex("LecturerId");

                    b.HasIndex("StudentId");

                    b.ToTable("CourseWorks");
                });

            modelBuilder.Entity("DataLayer.Entities.LecturerEntity.Lecturer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("MiddleName")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Token")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Lecturers");
                });

            modelBuilder.Entity("DataLayer.Entities.StudentEntity.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int?>("LecturerId")
                        .HasColumnType("integer");

                    b.Property<string>("MiddleName")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Token")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("LecturerId");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("DataLayer.Entities.TaskEntity.Task", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Comment")
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<int?>("CourseWorkId")
                        .HasColumnType("integer");

                    b.Property<string>("Priority")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CourseWorkId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("DataLayer.Entities.CourseWorkEntity.CourseWork", b =>
                {
                    b.HasOne("DataLayer.Entities.LecturerEntity.Lecturer", "Lecturer")
                        .WithMany("CourseWorks")
                        .HasForeignKey("LecturerId");

                    b.HasOne("DataLayer.Entities.StudentEntity.Student", "Student")
                        .WithMany("CourseWorks")
                        .HasForeignKey("StudentId");

                    b.Navigation("Lecturer");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("DataLayer.Entities.StudentEntity.Student", b =>
                {
                    b.HasOne("DataLayer.Entities.LecturerEntity.Lecturer", "Lecturer")
                        .WithMany("Students")
                        .HasForeignKey("LecturerId");

                    b.Navigation("Lecturer");
                });

            modelBuilder.Entity("DataLayer.Entities.TaskEntity.Task", b =>
                {
                    b.HasOne("DataLayer.Entities.CourseWorkEntity.CourseWork", "CourseWork")
                        .WithMany("Tasks")
                        .HasForeignKey("CourseWorkId");

                    b.Navigation("CourseWork");
                });

            modelBuilder.Entity("DataLayer.Entities.CourseWorkEntity.CourseWork", b =>
                {
                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("DataLayer.Entities.LecturerEntity.Lecturer", b =>
                {
                    b.Navigation("CourseWorks");

                    b.Navigation("Students");
                });

            modelBuilder.Entity("DataLayer.Entities.StudentEntity.Student", b =>
                {
                    b.Navigation("CourseWorks");
                });
#pragma warning restore 612, 618
        }
    }
}
