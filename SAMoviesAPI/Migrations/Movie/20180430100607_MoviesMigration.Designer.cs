﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using SAMoviesAPI.Contexts;
using System;

namespace SAMoviesAPI.Migrations.Movie
{
    [DbContext(typeof(MovieContext))]
    [Migration("20180430100607_MoviesMigration")]
    partial class MoviesMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SAMoviesAPI.Models.Comment", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content");

                    b.Property<int?>("MovieId");

                    b.HasKey("UserId");

                    b.HasIndex("MovieId");

                    b.ToTable("Comment");
                });

            modelBuilder.Entity("SAMoviesAPI.Models.Movie", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.HasKey("Id");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("SAMoviesAPI.Models.Rating", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("MovieId");

                    b.Property<int>("Rate");

                    b.HasKey("UserId");

                    b.HasIndex("MovieId");

                    b.ToTable("Rating");
                });

            modelBuilder.Entity("SAMoviesAPI.Models.Comment", b =>
                {
                    b.HasOne("SAMoviesAPI.Models.Movie")
                        .WithMany("Comments")
                        .HasForeignKey("MovieId");
                });

            modelBuilder.Entity("SAMoviesAPI.Models.Rating", b =>
                {
                    b.HasOne("SAMoviesAPI.Models.Movie")
                        .WithMany("Ratings")
                        .HasForeignKey("MovieId");
                });
#pragma warning restore 612, 618
        }
    }
}