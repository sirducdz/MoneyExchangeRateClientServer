﻿// <auto-generated />
using System;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataLayer.Migrations
{
    [DbContext(typeof(Prn221ProjectContext))]
    [Migration("20240320184031_initialDb")]
    partial class initialDb
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasAnnotation("ProductVersion", "6.0.28")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.HasCharSet(modelBuilder, "utf8mb4");

            modelBuilder.Entity("DataLayer.Models.Currency", b =>
                {
                    b.Property<int>("CurrencyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("CurrencyID");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("varchar(3)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<decimal>("Price")
                        .HasPrecision(19, 4)
                        .HasColumnType("decimal(19,4)");

                    b.HasKey("CurrencyId");

                    b.HasIndex(new[] { "Code" }, "Code")
                        .IsUnique();

                    b.ToTable("Currencies");
                });

            modelBuilder.Entity("DataLayer.Models.RateHistory", b =>
                {
                    b.Property<int>("ExchangeRateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ExchangeRateID");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime");

                    b.Property<int>("SourceCurrencyId")
                        .HasColumnType("int")
                        .HasColumnName("SourceCurrencyID");

                    b.Property<int>("TargetCurrencyId")
                        .HasColumnType("int")
                        .HasColumnName("TargetCurrencyID");

                    b.HasKey("ExchangeRateId")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "SourceCurrencyId" }, "FK_SourceCurrency");

                    b.HasIndex(new[] { "TargetCurrencyId" }, "FK_TargetCurrency");

                    b.ToTable("RateHistories");
                });

            modelBuilder.Entity("DataLayer.Models.User", b =>
                {
                    b.Property<string>("Email")
                        .HasColumnType("varchar(255)")
                        .HasColumnName("email")
                        .UseCollation("utf8mb3_general_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("Email"), "utf8mb3");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("password")
                        .UseCollation("utf8mb3_general_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("Password"), "utf8mb3");

                    b.HasKey("Email")
                        .HasName("PRIMARY");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DataLayer.Models.RateHistory", b =>
                {
                    b.HasOne("DataLayer.Models.Currency", "SourceCurrency")
                        .WithMany("RateHistorySourceCurrencies")
                        .HasForeignKey("SourceCurrencyId")
                        .IsRequired()
                        .HasConstraintName("FK_SourceCurrency");

                    b.HasOne("DataLayer.Models.Currency", "TargetCurrency")
                        .WithMany("RateHistoryTargetCurrencies")
                        .HasForeignKey("TargetCurrencyId")
                        .IsRequired()
                        .HasConstraintName("FK_TargetCurrency");

                    b.Navigation("SourceCurrency");

                    b.Navigation("TargetCurrency");
                });

            modelBuilder.Entity("DataLayer.Models.Currency", b =>
                {
                    b.Navigation("RateHistorySourceCurrencies");

                    b.Navigation("RateHistoryTargetCurrencies");
                });
#pragma warning restore 612, 618
        }
    }
}
