using Hydra.Module.Video.Backend.Models;

namespace Hydra.Module.Video.Backend.Extensions
{
    public static class FileChunkExtension
    {
        public static bool IsLastChunk(this FileChunk fileChunk)
        {
            return fileChunk.FullSize == fileChunk.Offset + fileChunk.Data.Length;
        }
    }
}