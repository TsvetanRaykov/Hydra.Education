using Hydra.Module.Video.Backend.Models;

namespace Hydra.Module.Video.Backend.Contracts
{
    public interface IChunk
    {
        FileChunk FileChunk { get; set; }
    }
}