﻿// <auto-generated />
using System;
using System.Diagnostics.CodeAnalysis;
using Cas.Account.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Cas.Account.DataAccess.Migrations
{
    [ExcludeFromCodeCoverage]
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.4");

            modelBuilder.Entity("Cas.Account.Core.Models.AccountModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<long>("CustomerId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Accounts");
                });
#pragma warning restore 612, 618
        }
    }
}
