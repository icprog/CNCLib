﻿// <auto-generated />
using System;
using CNCLib.Repository.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CNCLib.Repository.SqlServer.Migrations
{
    [DbContext(typeof(MigrationCNCLibContext))]
    partial class MigrationCNCLibContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CNCLib.Repository.Contracts.Entities.Configuration", b =>
                {
                    b.Property<string>("Group")
                        .HasMaxLength(256);

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<int?>("UserId");

                    b.Property<string>("Value")
                        .HasMaxLength(4000);

                    b.HasKey("Group", "Name");

                    b.HasIndex("UserId");

                    b.ToTable("Configuration");
                });

            modelBuilder.Entity("CNCLib.Repository.Contracts.Entities.Item", b =>
                {
                    b.Property<int>("ItemId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClassName")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<int?>("UserId");

                    b.HasKey("ItemId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("Item");
                });

            modelBuilder.Entity("CNCLib.Repository.Contracts.Entities.ItemProperty", b =>
                {
                    b.Property<int>("ItemId");

                    b.Property<string>("Name")
                        .HasMaxLength(255);

                    b.Property<string>("Value");

                    b.HasKey("ItemId", "Name");

                    b.ToTable("ItemProperty");
                });

            modelBuilder.Entity("CNCLib.Repository.Contracts.Entities.Machine", b =>
                {
                    b.Property<int>("MachineId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Axis");

                    b.Property<int>("BaudRate");

                    b.Property<int>("BufferSize");

                    b.Property<string>("ComPort")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<int>("CommandSyntax");

                    b.Property<bool>("CommandToUpper");

                    b.Property<bool>("Coolant");

                    b.Property<bool>("DtrIsReset");

                    b.Property<bool>("Laser");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<bool>("NeedDtr");

                    b.Property<decimal>("ProbeDist");

                    b.Property<decimal>("ProbeDistUp");

                    b.Property<decimal>("ProbeFeed");

                    b.Property<decimal>("ProbeSizeX");

                    b.Property<decimal>("ProbeSizeY");

                    b.Property<decimal>("ProbeSizeZ");

                    b.Property<bool>("Rotate");

                    b.Property<bool>("SDSupport");

                    b.Property<string>("SerialServer");

                    b.Property<int>("SerialServerPort");

                    b.Property<decimal>("SizeA");

                    b.Property<decimal>("SizeB");

                    b.Property<decimal>("SizeC");

                    b.Property<decimal>("SizeX");

                    b.Property<decimal>("SizeY");

                    b.Property<decimal>("SizeZ");

                    b.Property<bool>("Spindle");

                    b.Property<int?>("UserId");

                    b.HasKey("MachineId");

                    b.HasIndex("UserId");

                    b.ToTable("Machine");
                });

            modelBuilder.Entity("CNCLib.Repository.Contracts.Entities.MachineCommand", b =>
                {
                    b.Property<int>("MachineCommandId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CommandName")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<string>("CommandString")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<string>("JoystickMessage")
                        .HasMaxLength(64);

                    b.Property<int>("MachineId");

                    b.Property<int?>("PosX");

                    b.Property<int?>("PosY");

                    b.HasKey("MachineCommandId");

                    b.HasIndex("MachineId");

                    b.ToTable("MachineCommand");
                });

            modelBuilder.Entity("CNCLib.Repository.Contracts.Entities.MachineInitCommand", b =>
                {
                    b.Property<int>("MachineInitCommandId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CommandString")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<int>("MachineId");

                    b.Property<int>("SeqNo");

                    b.HasKey("MachineInitCommandId");

                    b.HasIndex("MachineId");

                    b.ToTable("MachineInitCommand");
                });

            modelBuilder.Entity("CNCLib.Repository.Contracts.Entities.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .IsUnicode(true);

                    b.Property<string>("UserPassword")
                        .HasMaxLength(255)
                        .IsUnicode(true);

                    b.HasKey("UserId");

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("User");
                });

            modelBuilder.Entity("Framework.Contracts.Repository.Entities.Log", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Application")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(true);

                    b.Property<string>("Exception")
                        .IsUnicode(true);

                    b.Property<string>("Level")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(true);

                    b.Property<DateTime>("LogDate");

                    b.Property<string>("Logger")
                        .HasMaxLength(250)
                        .IsUnicode(true);

                    b.Property<string>("MachineName")
                        .HasMaxLength(64)
                        .IsUnicode(true);

                    b.Property<string>("Message")
                        .IsRequired()
                        .IsUnicode(true);

                    b.Property<string>("Port")
                        .HasMaxLength(256)
                        .IsUnicode(true);

                    b.Property<string>("RemoteAddress")
                        .HasMaxLength(100)
                        .IsUnicode(true);

                    b.Property<string>("ServerAddress")
                        .HasMaxLength(100)
                        .IsUnicode(true);

                    b.Property<string>("ServerName")
                        .HasMaxLength(64)
                        .IsUnicode(true);

                    b.Property<string>("StackTrace")
                        .IsUnicode(true);

                    b.Property<string>("Url")
                        .HasMaxLength(500)
                        .IsUnicode(true);

                    b.Property<string>("UserName")
                        .HasMaxLength(250)
                        .IsUnicode(true);

                    b.HasKey("Id");

                    b.ToTable("Log");
                });

            modelBuilder.Entity("CNCLib.Repository.Contracts.Entities.Configuration", b =>
                {
                    b.HasOne("CNCLib.Repository.Contracts.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("CNCLib.Repository.Contracts.Entities.Item", b =>
                {
                    b.HasOne("CNCLib.Repository.Contracts.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("CNCLib.Repository.Contracts.Entities.ItemProperty", b =>
                {
                    b.HasOne("CNCLib.Repository.Contracts.Entities.Item", "Item")
                        .WithMany("ItemProperties")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CNCLib.Repository.Contracts.Entities.Machine", b =>
                {
                    b.HasOne("CNCLib.Repository.Contracts.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("CNCLib.Repository.Contracts.Entities.MachineCommand", b =>
                {
                    b.HasOne("CNCLib.Repository.Contracts.Entities.Machine", "Machine")
                        .WithMany("MachineCommands")
                        .HasForeignKey("MachineId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CNCLib.Repository.Contracts.Entities.MachineInitCommand", b =>
                {
                    b.HasOne("CNCLib.Repository.Contracts.Entities.Machine", "Machine")
                        .WithMany("MachineInitCommands")
                        .HasForeignKey("MachineId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
