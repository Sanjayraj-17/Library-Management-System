using System;
using System.Collections.Generic;
using System.Text;


using LibraryManagementEFCore.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementEFCore.Data;

public class AppDbContext : DbContext
{
    public const string ConnectionString =
        @"Server=.\SQLEXPRESS;Database=LibraryManagementEFCoreDB;Trusted_Connection=True;TrustServerCertificate=True;";

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // Database tables
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<Loan> Loans => Set<Loan>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // BOOK CONFIGURATION
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(b => b.BookId);

            entity.Property(b => b.Title)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(b => b.Author)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(b => b.ISBN)
                .HasMaxLength(20);

            entity.Property(b => b.Category)
                .HasMaxLength(60);

            entity.Property(b => b.Price)
                .HasColumnType("decimal(10,2)");

            entity.HasIndex(b => b.ISBN)
                .IsUnique()
                .HasFilter("[ISBN] IS NOT NULL");
        });

        // MEMBER CONFIGURATION
        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(m => m.MemberId);

            entity.Property(m => m.FullName)
                .HasMaxLength(120)
                .IsRequired();

            entity.Property(m => m.Email)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(m => m.Phone)
                .HasMaxLength(20);

            entity.HasIndex(m => m.Email)
                .IsUnique();
        });

        // LOAN CONFIGURATION
        modelBuilder.Entity<Loan>(entity =>
        {
            entity.HasKey(l => l.LoanId);

            entity.Property(l => l.FineAmount)
                .HasColumnType("decimal(10,2)");

            // One book can have many loan records.
            entity.HasOne(l => l.Book)
                .WithMany(b => b.Loans)
                .HasForeignKey(l => l.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            // One member can have many loan records.
            entity.HasOne(l => l.Member)
                .WithMany(m => m.Loans)
                .HasForeignKey(l => l.MemberId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(a => a.AuditLogId);

            entity.Property(a => a.Action)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(a => a.EntityName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(a => a.Description)
                .IsRequired()
                .HasMaxLength(1000);

            entity.Property(a => a.PerformedBy)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasIndex(a => a.CreatedAt);
        });
    }
}

