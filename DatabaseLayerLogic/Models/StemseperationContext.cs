﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DatabaseLayerLogic.Models;

public partial class StemseperationContext : DbContext
{
    public StemseperationContext()
    {
    }

    public StemseperationContext(DbContextOptions<StemseperationContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserFile> UserFiles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=localhost,1433;Initial Catalog=STEMSeperation;User ID=sa;Password=Karthik123;Encrypt=False;TrustServerCertificate=True");
    //Server=DESKTOP-1CFLAAS\\SQLEXPRESS;Database=STEMSeperation;Encrypt=False;TrustServerCertificate=True;Trusted_Connection=True;
    // Server=localhost,1433;Initial Catalog=STEMSeperation;User ID=sa;Password=Karthik123;Encrypt=False;TrustServerCertificate=True
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Pkuser).HasName("PK__Users__1EAE3C7906B3C1DB");

            entity.Property(e => e.LastLogIn).HasColumnType("datetime");
            entity.Property(e => e.LastPasswordChange).HasColumnType("datetime");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SaltValue)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserCreatedOn).HasColumnType("datetime");
            entity.Property(e => e.UserName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserFile>(entity =>
        {
            entity.HasKey(e => e.InstanceId).HasName("PK__UserFile__5C51996F40D80812");

            entity.Property(e => e.InstanceId).HasColumnName("InstanceID");
            entity.Property(e => e.InputPath)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.InstanceTime).HasColumnType("datetime");
            entity.Property(e => e.Stem1)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Stem2)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Stem3)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Stem4)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Stem5)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.UserFiles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_USERFILES_Users_UserId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
