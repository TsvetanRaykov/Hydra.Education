using Hydra.Module.Video.Models;

namespace Hydra.Module.Video.Contracts
{
    public interface IChunk
    {
        FileChunk FileChunk { get; set; }
    }
}