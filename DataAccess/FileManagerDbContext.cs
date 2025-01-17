﻿using DataAccess.DTO;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class FileManagerDbContext(DbContextOptions<FileManagerDbContext> options) : DbContext(options)
    {
        public DbSet<FileItem> FileItems { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<FileVersion> FileVersions { get; set; }
        

        public override int SaveChanges()
        {
            SetTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void SetTimestamps()
        {
            var entries = ChangeTracker.Entries().Where(e => e.Entity is FileItem || e.Entity is Folder &&
                (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    ((dynamic)entry.Entity).DateCreated = DateTime.UtcNow;
                }
                ((dynamic)entry.Entity).DateChanged = DateTime.UtcNow;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

          
            modelBuilder.Entity<Folder>()
                .HasMany(f => f.Files)
                .WithOne(f => f.Folder)
                .HasForeignKey(f => f.FolderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>().
                HasIndex(u=>u.Username).
                IsUnique();

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired();
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.PasswordSalt).IsRequired();
                entity.Property(e => e.Role).IsRequired(); // Ensure this is required
            });


           modelBuilder.Entity<User>()
                .HasMany<FileItem>()
                .WithOne()
                .HasForeignKey(f => f.CreatedBy)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany<Folder>()
                .WithOne()
                .HasForeignKey(f => f.CreatedBy)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FileVersion>()
                .HasOne(fv => fv.FileItem)
                .WithMany(fi => fi.FileVersions)
                .HasForeignKey(fv => fv.FileItemId);
        }
    }
}