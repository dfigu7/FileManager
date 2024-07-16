// FileManager.DataAccess/FileManagerDbContext.cs


using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

public class FileManagerDbContext(DbContextOptions<FileManagerDbContext> options) : DbContext(options)
{
    public DbSet<FileItem> Files { get; set; }
    public DbSet<Folder> Folders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Folder>()
            .HasMany(f => f.SubFolders)
            .WithOne(f => f.ParentFolder)
            .HasForeignKey(f => f.ParentFolderId);

        modelBuilder.Entity<Folder>()
            .HasMany(f => f.Files)
            .WithOne(f => f.Folder)
            .HasForeignKey(f => f.FolderId);
    }
}