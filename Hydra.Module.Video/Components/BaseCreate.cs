namespace Hydra.Module.Video.Components
{
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Forms;
    using Models;
    using System;
    using System.Threading.Tasks;

    public abstract class BaseCreate : ComponentBase
    {
        public abstract IManagedItem ManagedItem { get; set; }
        protected string ImageUrl { get; set; } 

        protected BaseCreate()
        {
            ImageUrl = "data:image/gif;base64,R0lGODlhAQABAAD/ACwAAAAAAQABAAACADs%3D";
        }

        protected async Task OnImageChange(InputFileChangeEventArgs args)
        {
            const string format = "image/png";
            var resizedImageFile = await args.File.RequestImageFileAsync(format, 150, 150);

            var buffer = new byte[resizedImageFile.Size];
            await resizedImageFile.OpenReadStream().ReadAsync(buffer);

            ImageUrl = $"data:image/png;base64,{Convert.ToBase64String(buffer)}";
            ManagedItem.Image = buffer;
        }
    }
}