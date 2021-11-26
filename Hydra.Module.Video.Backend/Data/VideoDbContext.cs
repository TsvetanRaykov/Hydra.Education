using Microsoft.EntityFrameworkCore;

namespace Hydra.Module.Video.Backend.Data
{
    public class VideoDbContext : DbContext
    {
        public DbSet<VideoClass> VideoClasses { get; set; }
        public DbSet<VideoGroup> VideoGroups { get; set; }

        public VideoDbContext(DbContextOptions<VideoDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VideoGroup>()
                .HasOne(g => g.VideoClass)
                .WithMany(c => c.VideoGroups)
                .HasForeignKey(c => c.VideoClassId);

            modelBuilder.Entity<UserToGroup>()
                .HasOne(g => g.VideoGroup)
                .WithMany(u => u.Users)
                .HasForeignKey(u => u.GroupId);

            modelBuilder.Entity<UserToGroup>()
                .HasKey(k => new { k.UserId, k.GroupId });

            base.OnModelCreating(modelBuilder);
        }
    }
}