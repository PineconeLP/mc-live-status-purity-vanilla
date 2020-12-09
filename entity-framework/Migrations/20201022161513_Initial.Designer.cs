﻿// <auto-generated />
using MCLiveStatus.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MCLiveStatus.EntityFramework.Migrations
{
    [DbContext(typeof(MCLiveStatusDbContext))]
    [Migration("20201022161513_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9");

            modelBuilder.Entity("MCLiveStatus.EntityFramework.Models.ServerPingerSettingsDTO", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("AllowNotifyJoinable")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("AllowNotifyQueueJoinable")
                        .HasColumnType("INTEGER");

                    b.Property<double>("PingIntervalSeconds")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("ServerPingerSettings");
                });
#pragma warning restore 612, 618
        }
    }
}