using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class FileManagerDbContext(DbContextOptions<FileManagerDbContext> options) : DbContext(options)
    {
        public DbSet<FileItem> Files { get; set; }
        public DbSet<Folder> Folders { get; set; }
        

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
                .HasMany(f => f.SubFolders)
                .WithOne(f => f.ParentFolder)
                .HasForeignKey(f => f.ParentFolderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}