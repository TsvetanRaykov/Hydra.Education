using System.Collections.Generic;
using Hydra.Module.Video.Backend.Data;

namespace Hydra.Module.Video.Backend.Models
{
    public class GroupResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public VideoClass Class { get; set; }

        public List<string> Users { get; set; } = new();
        public List<PlaylistResponseDto> Playlists { get; set; }

    }
}