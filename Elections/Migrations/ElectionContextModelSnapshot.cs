﻿// <auto-generated />
using Elections.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Elections.Migrations
{
    [DbContext(typeof(ElectionContext))]
    partial class ElectionContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Elections.Models.Candidate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Accepted");

                    b.Property<string>("Background")
                        .IsRequired()
                        .HasMaxLength(5000);

                    b.Property<string>("Church")
                        .IsRequired();

                    b.Property<bool>("Confirmed");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("getdate()");

                    b.Property<int>("ElectionYear");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<Guid>("Guid");

                    b.Property<bool>("Ignored");

                    b.Property<string>("ImageUrl");

                    b.Property<string>("Location")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("PastorContact");

                    b.Property<int>("Position");

                    b.Property<bool>("Ready");

                    b.Property<string>("Reasons")
                        .IsRequired()
                        .HasMaxLength(5000);

                    b.Property<bool>("Selected");

                    b.Property<string>("Submitter")
                        .IsRequired();

                    b.Property<string>("SubmitterEmail")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Candidates");
                });

            modelBuilder.Entity("Elections.Models.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CandidateId");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(5000);

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("Submitter")
                        .IsRequired();

                    b.Property<string>("SubmitterEmail")
                        .IsRequired();

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("CandidateId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("Elections.Models.Election", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("ElectionDay");

                    b.Property<DateTime>("NominationCutoff");

                    b.Property<bool>("NominationsOpen");

                    b.Property<bool>("VotingOpen");

                    b.Property<int>("Year");

                    b.HasKey("Id");

                    b.ToTable("Elections");
                });

            modelBuilder.Entity("Elections.Models.Vote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CandidateId");

                    b.Property<bool>("For");

                    b.Property<int?>("VoterId");

                    b.HasKey("Id");

                    b.HasIndex("CandidateId");

                    b.HasIndex("VoterId");

                    b.ToTable("Votes");
                });

            modelBuilder.Entity("Elections.Models.Voter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<string>("Email");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Voters");
                });

            modelBuilder.Entity("Elections.Models.Comment", b =>
                {
                    b.HasOne("Elections.Models.Candidate", "Candidate")
                        .WithMany()
                        .HasForeignKey("CandidateId");
                });

            modelBuilder.Entity("Elections.Models.Vote", b =>
                {
                    b.HasOne("Elections.Models.Candidate", "Candidate")
                        .WithMany()
                        .HasForeignKey("CandidateId");

                    b.HasOne("Elections.Models.Voter", "Voter")
                        .WithMany()
                        .HasForeignKey("VoterId");
                });
#pragma warning restore 612, 618
        }
    }
}
