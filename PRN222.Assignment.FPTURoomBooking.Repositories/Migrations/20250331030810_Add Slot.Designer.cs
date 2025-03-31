﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PRN222.Assignment.FPTURoomBooking.Repositories.Models;

#nullable disable

namespace PRN222.Assignment.FPTURoomBooking.Repositories.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250331030810_Add Slot")]
    partial class AddSlot
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PRN222.Assignment.FPTURoomBooking.Repositories.Models.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("DepartmentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("FullName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Account", (string)null);
                });

            modelBuilder.Entity("PRN222.Assignment.FPTURoomBooking.Repositories.Models.Booking", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("BookingDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<Guid?>("ManagerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("Booking", (string)null);
                });

            modelBuilder.Entity("PRN222.Assignment.FPTURoomBooking.Repositories.Models.Campus", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Campus", (string)null);
                });

            modelBuilder.Entity("PRN222.Assignment.FPTURoomBooking.Repositories.Models.Department", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CampusId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CampusId");

                    b.ToTable("Department", (string)null);
                });

            modelBuilder.Entity("PRN222.Assignment.FPTURoomBooking.Repositories.Models.Room", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CampusId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("DepartmentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CampusId");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Room", (string)null);
                });

            modelBuilder.Entity("PRN222.Assignment.FPTURoomBooking.Repositories.Models.RoomSlot", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BookingId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("TimeSlot")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("BookingId");

                    b.HasIndex("RoomId");

                    b.ToTable("RoomSlot", (string)null);
                });

            modelBuilder.Entity("PRN222.Assignment.FPTURoomBooking.Repositories.Models.Slot", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BookingId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("BookingId");

                    b.HasIndex("RoomId");

                    b.ToTable("Slot", (string)null);
                });

            modelBuilder.Entity("PRN222.Assignment.FPTURoomBooking.Repositories.Models.Account", b =>
                {
                    b.HasOne("PRN222.Assignment.FPTURoomBooking.Repositories.Models.Department", "Department")
                        .WithMany("Accounts")
                        .HasForeignKey("DepartmentId");

                    b.Navigation("Department");
                });

            modelBuilder.Entity("PRN222.Assignment.FPTURoomBooking.Repositories.Models.Booking", b =>
                {
                    b.HasOne("PRN222.Assignment.FPTURoomBooking.Repositories.Models.Account", "Account")
                        .WithMany("Bookings")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("PRN222.Assignment.FPTURoomBooking.Repositories.Models.Department", b =>
                {
                    b.HasOne("PRN222.Assignment.FPTURoomBooking.Repositories.Models.Campus", "Campus")
                        .WithMany("Departments")
                        .HasForeignKey("CampusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Campus");
                });

            modelBuilder.Entity("PRN222.Assignment.FPTURoomBooking.Repositories.Models.Room", b =>
                {
                    b.HasOne("PRN222.Assignment.FPTURoomBooking.Repositories.Models.Campus", "Campus")
                        .WithMany("Rooms")
                        .HasForeignKey("CampusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PRN222.Assignment.FPTURoomBooking.Repositories.Models.Department", null)
                        .WithMany("Rooms")
                        .HasForeignKey("DepartmentId");

                    b.Navigation("Campus");
                });

            modelBuilder.Entity("PRN222.Assignment.FPTURoomBooking.Repositories.Models.RoomSlot", b =>
                {
                    b.HasOne("PRN222.Assignment.FPTURoomBooking.Repositories.Models.Booking", "Booking")
                        .WithMany("RoomSlots")
                        .HasForeignKey("BookingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PRN222.Assignment.FPTURoomBooking.Repositories.Models.Room", "Room")
                        .WithMany("RoomSlots")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Booking");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("PRN222.Assignment.FPTURoomBooking.Repositories.Models.Slot", b =>
                {
                    b.HasOne("PRN222.Assignment.FPTURoomBooking.Repositories.Models.Booking", "Booking")
                        .WithMany()
                        .HasForeignKey("BookingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PRN222.Assignment.FPTURoomBooking.Repositories.Models.Room", "Room")
                        .WithMany("Slots")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Booking");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("PRN222.Assignment.FPTURoomBooking.Repositories.Models.Account", b =>
                {
                    b.Navigation("Bookings");
                });

            modelBuilder.Entity("PRN222.Assignment.FPTURoomBooking.Repositories.Models.Booking", b =>
                {
                    b.Navigation("RoomSlots");
                });

            modelBuilder.Entity("PRN222.Assignment.FPTURoomBooking.Repositories.Models.Campus", b =>
                {
                    b.Navigation("Departments");

                    b.Navigation("Rooms");
                });

            modelBuilder.Entity("PRN222.Assignment.FPTURoomBooking.Repositories.Models.Department", b =>
                {
                    b.Navigation("Accounts");

                    b.Navigation("Rooms");
                });

            modelBuilder.Entity("PRN222.Assignment.FPTURoomBooking.Repositories.Models.Room", b =>
                {
                    b.Navigation("RoomSlots");

                    b.Navigation("Slots");
                });
#pragma warning restore 612, 618
        }
    }
}
