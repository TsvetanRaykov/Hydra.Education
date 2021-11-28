namespace Hydra.Module.Video.Backend.Services
{
    using Contracts;
    using Models;
    using System;
    using System.IO;
    using System.Threading.Tasks;

    public class FileService : IFileService
    {
        public Task<string> WriteFileChunkAsync(string fullPath, FileChunk fileChunk)
        {
            try
            {

                if (fileChunk.FirstChunk && System.IO.File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }

                using var stream = File.OpenWrite(fullPath);

                stream.Seek(fileChunk.Offset, SeekOrigin.Begin);
                stream.Write(fileChunk.Data, 0, fileChunk.Data.Length);

                return Task.FromResult(string.Empty);
            }
            catch (Exception ex)
            {
                return Task.FromResult(ex.Message);
            }
        }
    }
}