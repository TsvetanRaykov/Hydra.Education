namespace Hydra.Module.Video.Components
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Models;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Forms;

    public class BaseCreate : ComponentBase
    {
        protected FileChunk FileChunk { get; private set; }
        protected string ImageUrl { get; private set; }

        public BaseCreate()
        {
            ImageUrl = "data:image/gif;base64,R0lGODlhAQABAAD/ACwAAAAAAQABAAACADs%3D";
        }

        protected async Task OnInputFileChange(InputFileChangeEventArgs args)
        {
            const string format = "image/png";
            var resizedImageFile = await args.File.RequestImageFileAsync(format, 150, 150);

            var buffer = new byte[resizedImageFile.Size];
            await resizedImageFile.OpenReadStream().ReadAsync(buffer);

            var justFileName = Path.GetFileNameWithoutExtension(args.File.Name);
            var newFileNameWithoutPath = $"{justFileName}-{DateTime.Now.Ticks.ToString()}.png";

            this.FileChunk = new FileChunk
            {
                Data = buffer,
                FileNameNoPath = newFileNameWithoutPath,
                Offset = 0,
                FirstChunk = true
            };

            this.ImageUrl = $"data:image/png;base64,{Convert.ToBase64String(FileChunk.Data)}";
        }
    }
}