namespace Hydra.Module.Video.Backend.Models
{
    public class FileChunk
    {
        public string FileNameNoPath { get; set; }
        public long Offset { get; set; }
        public byte[] Data { get; set; }
        public bool FirstChunk { get; set; }
        public long FullSize { get; set; }
    }
}