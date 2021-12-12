using Hydra.Module.Video.Backend.Contracts;

namespace Hydra.Module.Video.Backend.Models
{
    public class VideoRequestDto : IChunk
    {
        public string Name { get; set; }
        public int[] Playlists { get; set; }
        public string Description { get; set; }
        public FileChunk FileChunk { get; set; }
    }
}