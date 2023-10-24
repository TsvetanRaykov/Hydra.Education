using Hydra.Module.Video.Backend.Contracts;

namespace Hydra.Module.Video.Backend.Models
{
    public class UploadFileRequest : IChunk
    {
        public FileChunk FileChunk { get; set; }
    }
}