﻿// <auto-generated />
using System;
using AGSR.TestTask.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AGSR.TestTask.Migrations
{
    [DbContext(typeof(AgsrContext))]
    [Migration("20241224061133_GivenAsArray")]
    partial class GivenAsArray
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.36")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AGSR.TestTask.Models.Patient", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<bool?>("Active")
                        .HasColumnType("boolean")
                        .HasColumnName("active");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("birth_date");

                    b.Property<string>("Family")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("family");

                    b.Property<int?>("Gender")
                        .HasColumnType("integer")
                        .HasColumnName("gender");

                    b.Property<string[]>("Given")
                        .HasColumnType("text[]")
                        .HasColumnName("given");

                    b.Property<string>("Use")
                        .HasColumnType("text")
                        .HasColumnName("use");

                    b.HasKey("Id");

                    b.HasIndex("BirthDate");

                    b.HasIndex("Family");

                    b.ToTable("patient", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
