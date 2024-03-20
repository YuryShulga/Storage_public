﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Storage.Data.StorageDb;

#nullable disable

namespace Storage.Data.StorageDb.StorageDbMigrations
{
    [DbContext(typeof(StorageDbContext))]
    partial class StorageDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Storage.Data.StorageDb.Location", b =>
                {
                    b.Property<int>("LocationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LocationId"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("RoomId")
                        .HasColumnType("int");

                    b.Property<int?>("StoragePlaceCellId")
                        .HasColumnType("int");

                    b.Property<int?>("StoragePlaceId")
                        .HasColumnType("int");

                    b.HasKey("LocationId");

                    b.HasIndex("RoomId");

                    b.HasIndex("StoragePlaceCellId");

                    b.HasIndex("StoragePlaceId");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("Storage.Data.StorageDb.Room", b =>
                {
                    b.Property<int>("RoomId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoomId"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RoomId");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("Storage.Data.StorageDb.StorageItem", b =>
                {
                    b.Property<int>("StorageItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StorageItemId"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("InitDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("LocationId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("StorageItemId");

                    b.HasIndex("LocationId");

                    b.HasIndex("UserId");

                    b.ToTable("StorageItems");
                });

            modelBuilder.Entity("Storage.Data.StorageDb.StoragePlace", b =>
                {
                    b.Property<int>("StoragePlaceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StoragePlaceId"));

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("StoragePlaceId");

                    b.ToTable("StoragePlaces");
                });

            modelBuilder.Entity("Storage.Data.StorageDb.StoragePlaceCell", b =>
                {
                    b.Property<int>("StoragePlaceCellId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StoragePlaceCellId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("StoragePlaceCellId");

                    b.ToTable("StoragePlacesCells");
                });

            modelBuilder.Entity("Storage.Data.StorageDb.User", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Storage.Data.StorageDb.Location", b =>
                {
                    b.HasOne("Storage.Data.StorageDb.Room", "Room")
                        .WithMany()
                        .HasForeignKey("RoomId");

                    b.HasOne("Storage.Data.StorageDb.StoragePlaceCell", "StoragePlaceCell")
                        .WithMany()
                        .HasForeignKey("StoragePlaceCellId");

                    b.HasOne("Storage.Data.StorageDb.StoragePlace", "StoragePlace")
                        .WithMany()
                        .HasForeignKey("StoragePlaceId");

                    b.Navigation("Room");

                    b.Navigation("StoragePlace");

                    b.Navigation("StoragePlaceCell");
                });

            modelBuilder.Entity("Storage.Data.StorageDb.StorageItem", b =>
                {
                    b.HasOne("Storage.Data.StorageDb.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Storage.Data.StorageDb.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Location");

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}
