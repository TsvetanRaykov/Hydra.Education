using System.Collections.Generic;

namespace Hydra.Module.Video.Backend.Data
{
    public class Playlist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string TrainerId { get; set; }

        public List<VideoToPlaylist> Videos { get; set; }
        public List<PlaylistToGroup> VideoGroups { get; set; }
    }
}