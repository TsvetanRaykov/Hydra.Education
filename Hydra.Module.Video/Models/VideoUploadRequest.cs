namespace Hydra.Module.Video.Models
{
    using Contracts;
    public class VideoUploadRequest : IChunk
    {
        public string Name { get; set; }
        public int[] Playlists { get; set; }
        public string Description { get; set; }
        public FileChunk FileChunk { get; set; }
    }
}