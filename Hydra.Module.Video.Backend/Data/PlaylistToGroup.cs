namespace Hydra.Module.Video.Backend.Data
{
    public class PlaylistToGroup
    {
        public int PlaylistId { get; set; }
        public virtual Playlist Playlist { get; set; }

        public int GroupId { get; set; }
        public virtual VideoGroup VideoGroup { get; set; }
    }
}