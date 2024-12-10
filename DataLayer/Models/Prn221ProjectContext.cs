using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace DataLayer.Models
{
    public partial class Prn221ProjectContext : DbContext
    {
        public Prn221ProjectContext()
        {
        }

        public Prn221ProjectContext(DbContextOptions<Prn221ProjectContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Currency> Currencies { get; set; } = null!;
        public virtual DbSet<RateHistory> RateHistories { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;


        //public string GetconnectionString()
        //{
        //    IConfiguration config = new ConfigurationBuilder()
        //        .SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile("appsettings.json", true, true)
        //        .Build();
        //    var strConnection = config["ConnectionStrings:Default"];
        //    return strConnection;
        //}


        //        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //        {
        //            if (!optionsBuilder.IsConfigured)
        //            {
        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        //                optionsBuilder.UseMySql("server=localhost;port=3306;user=root;password=Sirducdz7a@;database=Prn221Project", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.33-mysql"));
        //            }
        //        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Currency>(entity =>
            {
                //entity.ToTable("currency");

                entity.HasIndex(e => e.Code, "Code")
                    .IsUnique();

                entity.Property(e => e.CurrencyId).HasColumnName("CurrencyID");

                entity.Property(e => e.Code).HasMaxLength(3);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Price).HasPrecision(19, 4);
            });

            modelBuilder.Entity<RateHistory>(entity =>
            {
                entity.HasKey(e => e.ExchangeRateId)
                    .HasName("PRIMARY");

                //entity.ToTable("RateHistory");

                entity.HasIndex(e => e.SourceCurrencyId, "FK_SourceCurrency");

                entity.HasIndex(e => e.TargetCurrencyId, "FK_TargetCurrency");

                entity.Property(e => e.ExchangeRateId).HasColumnName("ExchangeRateID");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.SourceCurrencyId).HasColumnName("SourceCurrencyID");

                //entity.Property(e => e.SourceCurrencyPrice).HasPrecision(19, 4);

                entity.Property(e => e.TargetCurrencyId).HasColumnName("TargetCurrencyID");

                //entity.Property(e => e.TargetCurrencyPrice).HasPrecision(19, 4);

                entity.HasOne(d => d.SourceCurrency)
                    .WithMany(p => p.RateHistorySourceCurrencies)
                    .HasForeignKey(d => d.SourceCurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SourceCurrency");

                entity.HasOne(d => d.TargetCurrency)
                    .WithMany(p => p.RateHistoryTargetCurrencies)
                    .HasForeignKey(d => d.TargetCurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TargetCurrency");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Email)
                    .HasName("PRIMARY");

                //entity.ToTable("user");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .UseCollation("utf8mb3_general_ci")
                    .HasCharSet("utf8mb3");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .HasColumnName("password")
                    .UseCollation("utf8mb3_general_ci")
                    .HasCharSet("utf8mb3");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
