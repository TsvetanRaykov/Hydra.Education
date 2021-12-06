using Hydra.Module.Video.Backend.Services;

namespace Hydra.Module.Video.Backend.Models
{
    using System.Collections.Generic;
    public class PlaylistResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public List<VideoResponseDto> Videos { get; set; }
        public List<GroupResponseDto> VideoGroups { get; set; }
    }
}