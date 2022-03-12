using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hydra.Module.Video.Backend.Data
{
    public class VideoGroup : BaseEntity
    {
        public string ImageUrl { get; set; }

        [Required]
        public int VideoClassId { get; set; }

        public VideoClass VideoClass { get; set; }

        public ICollection<UserToGroup> Users { get; set; }
        public ICollection<PlaylistToGroup> Playlists { get; set; }
    }
}