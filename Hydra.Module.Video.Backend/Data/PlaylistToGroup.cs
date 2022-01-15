namespace Hydra.Module.Video.Backend.Data
{
    public class PlaylistToGroup
    {
        public int PlaylistId { get; set; }
        public Playlist Playlist { get; set; }

        public int GroupId { get; set; }
        public VideoGroup Group { get; set; }
    }
}