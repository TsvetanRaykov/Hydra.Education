using System.Collections.Generic;

namespace Hydra.Module.Video.Backend.Data
{
    public class Playlist : BaseEntity
    {
        public string ImageUrl { get; set; }
        public string TrainerId { get; set; }

        public ICollection<VideoToPlaylist> Videos { get; set; }
        public ICollection<PlaylistToGroup> VideoGroups { get; set; }
    }
}