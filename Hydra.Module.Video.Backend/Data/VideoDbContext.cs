using Microsoft.EntityFrameworkCore;

namespace Hydra.Module.Video.Backend.Data
{
    public class VideoDbContext : DbContext
    {
        public DbSet<VideoClass> VideoClasses { get; set; }
        public DbSet<VideoGroup> VideoGroups { get; set; }

        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Video> Videos { get; set; }

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

            modelBuilder.Entity<PlaylistToGroup>()
                .HasOne(e => e.VideoGroup)
                .WithMany(p => p.Playlists);

            modelBuilder.Entity<PlaylistToGroup>()
                .HasOne(o => o.Playlist)
                .WithMany(o => o.VideoGroups);

            modelBuilder.Entity<PlaylistToGroup>()
                .HasKey(k => new { k.PlaylistId, k.GroupId });

            modelBuilder.Entity<VideoToPlaylist>()
                .HasOne(p => p.Playlist)
                .WithMany(v => v.Videos);

            modelBuilder.Entity<VideoToPlaylist>()
                .HasOne(v => v.Video)
                .WithMany(p => p.Playlists);

            modelBuilder.Entity<VideoToPlaylist>()
                .HasKey(k => new { k.PlaylistId, k.VideoId });

            base.OnModelCreating(modelBuilder);
        }
    }
}