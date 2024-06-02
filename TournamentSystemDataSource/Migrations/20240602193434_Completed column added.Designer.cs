﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TournamentSystemDataSource.Contexts;

#nullable disable

namespace TournamentSystemDataSource.Migrations
{
    [DbContext(typeof(GeneralContext))]
    [Migration("20240602193434_Completed column added")]
    partial class Completedcolumnadded
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TournamentSystemModels.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AppartmentNr")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("HouseNr")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostalCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Street")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("TournamentSystemModels.Matchup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<int>("MatchupRound")
                        .HasColumnType("int");

                    b.Property<int>("TournamentId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int?>("WinnerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TournamentId");

                    b.HasIndex("WinnerId");

                    b.ToTable("Matchups");
                });

            modelBuilder.Entity("TournamentSystemModels.MatchupEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ParentMatchupId")
                        .HasColumnType("int");

                    b.Property<double>("Score")
                        .HasColumnType("float");

                    b.Property<int?>("TeamCompetingId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ParentMatchupId");

                    b.HasIndex("TeamCompetingId");

                    b.ToTable("MatchupEntries");
                });

            modelBuilder.Entity("TournamentSystemModels.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AddressId")
                        .HasColumnType("int");

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Gender")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TeamId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UserPictureId")
                        .HasColumnType("int");

                    b.Property<double>("Weight")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("TeamId");

                    b.HasIndex("UserPictureId");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("TournamentSystemModels.Pictures", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("PictureUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Pictures");
                });

            modelBuilder.Entity("TournamentSystemModels.Prize", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("PlaceName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PlaceNr")
                        .HasColumnType("int");

                    b.Property<decimal>("PrizeAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PrizePercentage")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("TournamentId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("TournamentId");

                    b.ToTable("Prizes");
                });

            modelBuilder.Entity("TournamentSystemModels.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<int?>("DescriptionId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TournamentId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("DescriptionId");

                    b.HasIndex("TournamentId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("TournamentSystemModels.TeamDescription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SchoolNr")
                        .HasColumnType("int");

                    b.Property<int?>("TeamPictureId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("TeamPictureId");

                    b.ToTable("TeamsDescriptions");
                });

            modelBuilder.Entity("TournamentSystemModels.Tournament", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Completed")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("EntryFee")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("TournamentPictureId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("TournamentPictureId");

                    b.ToTable("Tournaments");
                });

            modelBuilder.Entity("TournamentSystemModels.Matchup", b =>
                {
                    b.HasOne("TournamentSystemModels.Tournament", null)
                        .WithMany("Rounds")
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TournamentSystemModels.Team", "Winner")
                        .WithMany()
                        .HasForeignKey("WinnerId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Winner");
                });

            modelBuilder.Entity("TournamentSystemModels.MatchupEntry", b =>
                {
                    b.HasOne("TournamentSystemModels.Matchup", "ParentMatchup")
                        .WithMany("Entries")
                        .HasForeignKey("ParentMatchupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TournamentSystemModels.Team", "TeamCompeting")
                        .WithMany()
                        .HasForeignKey("TeamCompetingId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("ParentMatchup");

                    b.Navigation("TeamCompeting");
                });

            modelBuilder.Entity("TournamentSystemModels.Person", b =>
                {
                    b.HasOne("TournamentSystemModels.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TournamentSystemModels.Team", null)
                        .WithMany("TeamMembers")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TournamentSystemModels.Pictures", "UserPicture")
                        .WithMany()
                        .HasForeignKey("UserPictureId");

                    b.Navigation("Address");

                    b.Navigation("UserPicture");
                });

            modelBuilder.Entity("TournamentSystemModels.Prize", b =>
                {
                    b.HasOne("TournamentSystemModels.Tournament", null)
                        .WithMany("Prizes")
                        .HasForeignKey("TournamentId");
                });

            modelBuilder.Entity("TournamentSystemModels.Team", b =>
                {
                    b.HasOne("TournamentSystemModels.TeamDescription", "Description")
                        .WithMany()
                        .HasForeignKey("DescriptionId");

                    b.HasOne("TournamentSystemModels.Tournament", null)
                        .WithMany("EnteredTeams")
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Description");
                });

            modelBuilder.Entity("TournamentSystemModels.TeamDescription", b =>
                {
                    b.HasOne("TournamentSystemModels.Pictures", "TeamPicture")
                        .WithMany()
                        .HasForeignKey("TeamPictureId");

                    b.Navigation("TeamPicture");
                });

            modelBuilder.Entity("TournamentSystemModels.Tournament", b =>
                {
                    b.HasOne("TournamentSystemModels.Pictures", "TournamentPicture")
                        .WithMany()
                        .HasForeignKey("TournamentPictureId");

                    b.Navigation("TournamentPicture");
                });

            modelBuilder.Entity("TournamentSystemModels.Matchup", b =>
                {
                    b.Navigation("Entries");
                });

            modelBuilder.Entity("TournamentSystemModels.Team", b =>
                {
                    b.Navigation("TeamMembers");
                });

            modelBuilder.Entity("TournamentSystemModels.Tournament", b =>
                {
                    b.Navigation("EnteredTeams");

                    b.Navigation("Prizes");

                    b.Navigation("Rounds");
                });
#pragma warning restore 612, 618
        }
    }
}
