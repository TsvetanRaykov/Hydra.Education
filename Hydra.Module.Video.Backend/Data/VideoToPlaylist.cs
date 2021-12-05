namespace Hydra.Module.Video.Backend.Data
{
    public class VideoToPlaylist
    {
        public int VideoId { get; set; }
        public virtual Video Video { get; set; }

        public int PlaylistId { get; set; }
        public virtual Playlist Playlist { get; set; }
    }
}