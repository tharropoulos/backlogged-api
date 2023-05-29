﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using backlogged_api.Data;

#nullable disable

namespace backlogged_api.Migrations
{
    [DbContext(typeof(BackloggedDBContext))]
    [Migration("20230529101834_AddBacklog")]
    partial class AddBacklog
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BacklogGame", b =>
                {
                    b.Property<Guid>("backlogsid")
                        .HasColumnType("uuid");

                    b.Property<Guid>("gamesid")
                        .HasColumnType("uuid");

                    b.HasKey("backlogsid", "gamesid");

                    b.HasIndex("gamesid");

                    b.ToTable("BacklogGame");
                });

            modelBuilder.Entity("DeveloperGame", b =>
                {
                    b.Property<Guid>("developersid")
                        .HasColumnType("uuid");

                    b.Property<Guid>("gamesid")
                        .HasColumnType("uuid");

                    b.HasKey("developersid", "gamesid");

                    b.HasIndex("gamesid");

                    b.ToTable("DeveloperGame");
                });

            modelBuilder.Entity("GameGenre", b =>
                {
                    b.Property<Guid>("gamesid")
                        .HasColumnType("uuid");

                    b.Property<Guid>("genresid")
                        .HasColumnType("uuid");

                    b.HasKey("gamesid", "genresid");

                    b.HasIndex("genresid");

                    b.ToTable("GameGenre");
                });

            modelBuilder.Entity("GamePlatform", b =>
                {
                    b.Property<Guid>("gamesid")
                        .HasColumnType("uuid");

                    b.Property<Guid>("platformsid")
                        .HasColumnType("uuid");

                    b.HasKey("gamesid", "platformsid");

                    b.HasIndex("platformsid");

                    b.ToTable("GamePlatform");
                });

            modelBuilder.Entity("backlogged_api.Models.Backlog", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<bool>("isVisible")
                        .HasColumnType("boolean");

                    b.Property<Guid>("userId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.HasKey("id");

                    b.HasIndex("userId")
                        .IsUnique();

                    b.ToTable("Backlog");
                });

            modelBuilder.Entity("backlogged_api.Models.Developer", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.ToTable("Developer");
                });

            modelBuilder.Entity("backlogged_api.Models.Franchise", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.ToTable("Franchises");
                });

            modelBuilder.Entity("backlogged_api.Models.Game", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("backgroundImageUrl")
                        .HasColumnType("text");

                    b.Property<string>("coverImageUrl")
                        .HasColumnType("text");

                    b.Property<string>("description")
                        .HasColumnType("text");

                    b.Property<Guid?>("franchiseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<Guid?>("publisherId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<float>("rating")
                        .HasColumnType("real");

                    b.Property<string>("title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.HasIndex("franchiseId");

                    b.HasIndex("publisherId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("backlogged_api.Models.Genre", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.ToTable("Genre");
                });

            modelBuilder.Entity("backlogged_api.Models.Platform", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.ToTable("Platform");
                });

            modelBuilder.Entity("backlogged_api.Models.Publisher", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.ToTable("Publisher");
                });

            modelBuilder.Entity("backlogged_api.Models.Review", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<Guid>("authorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<DateTime>("createdAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("details")
                        .HasColumnType("text");

                    b.Property<Guid>("gameId")
                        .HasColumnType("uuid");

                    b.Property<int>("rating")
                        .HasColumnType("integer");

                    b.HasKey("id");

                    b.HasIndex("authorId");

                    b.HasIndex("gameId");

                    b.ToTable("Review");
                });

            modelBuilder.Entity("backlogged_api.Models.User", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<Guid?>("backlogId")
                        .HasColumnType("uuid");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("lastName")
                        .HasColumnType("text");

                    b.Property<string>("passwordHash")
                        .HasColumnType("text");

                    b.Property<string>("profileImageUrl")
                        .HasColumnType("text");

                    b.Property<DateTime>("registeredAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("userName")
                        .HasColumnType("text");

                    b.Property<string>("username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.HasIndex("email")
                        .IsUnique();

                    b.ToTable("User");
                });

            modelBuilder.Entity("BacklogGame", b =>
                {
                    b.HasOne("backlogged_api.Models.Backlog", null)
                        .WithMany()
                        .HasForeignKey("backlogsid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("backlogged_api.Models.Game", null)
                        .WithMany()
                        .HasForeignKey("gamesid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DeveloperGame", b =>
                {
                    b.HasOne("backlogged_api.Models.Developer", null)
                        .WithMany()
                        .HasForeignKey("developersid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("backlogged_api.Models.Game", null)
                        .WithMany()
                        .HasForeignKey("gamesid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GameGenre", b =>
                {
                    b.HasOne("backlogged_api.Models.Game", null)
                        .WithMany()
                        .HasForeignKey("gamesid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("backlogged_api.Models.Genre", null)
                        .WithMany()
                        .HasForeignKey("genresid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GamePlatform", b =>
                {
                    b.HasOne("backlogged_api.Models.Game", null)
                        .WithMany()
                        .HasForeignKey("gamesid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("backlogged_api.Models.Platform", null)
                        .WithMany()
                        .HasForeignKey("platformsid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("backlogged_api.Models.Backlog", b =>
                {
                    b.HasOne("backlogged_api.Models.User", "user")
                        .WithOne("backlog")
                        .HasForeignKey("backlogged_api.Models.Backlog", "userId");

                    b.Navigation("user");
                });

            modelBuilder.Entity("backlogged_api.Models.Game", b =>
                {
                    b.HasOne("backlogged_api.Models.Franchise", "franchise")
                        .WithMany("games")
                        .HasForeignKey("franchiseId");

                    b.HasOne("backlogged_api.Models.Publisher", "publisher")
                        .WithMany("games")
                        .HasForeignKey("publisherId");

                    b.Navigation("franchise");

                    b.Navigation("publisher");
                });

            modelBuilder.Entity("backlogged_api.Models.Review", b =>
                {
                    b.HasOne("backlogged_api.Models.User", "author")
                        .WithMany("reviews")
                        .HasForeignKey("authorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("backlogged_api.Models.Game", "game")
                        .WithMany("reviews")
                        .HasForeignKey("gameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("author");

                    b.Navigation("game");
                });

            modelBuilder.Entity("backlogged_api.Models.Franchise", b =>
                {
                    b.Navigation("games");
                });

            modelBuilder.Entity("backlogged_api.Models.Game", b =>
                {
                    b.Navigation("reviews");
                });

            modelBuilder.Entity("backlogged_api.Models.Publisher", b =>
                {
                    b.Navigation("games");
                });

            modelBuilder.Entity("backlogged_api.Models.User", b =>
                {
                    b.Navigation("backlog");

                    b.Navigation("reviews");
                });
#pragma warning restore 612, 618
        }
    }
}
