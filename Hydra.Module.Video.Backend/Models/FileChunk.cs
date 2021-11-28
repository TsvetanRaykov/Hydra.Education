namespace Hydra.Module.Video.Backend.Models
{
    public class FileChunk
    {
        public string FileNameNoPath { get; set; } = string.Empty;
        public long Offset { get; set; }
        public byte[] Data { get; set; }
        public bool FirstChunk { get; set; }
    }
}