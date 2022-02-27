namespace Hydra.Module.Video.Components
{
    using Contracts;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Forms;
    using Resources;
    using System;
    using System.Threading.Tasks;

    public abstract class BaseCreate : ComponentBase
    {
        public abstract IManagedItem ManagedItem { get; set; }

        public abstract string ApiBaseUrl { get; }

        protected string ImageUrl
        {
            get
            {
                if (ManagedItem?.Image != null)
                {
                    return $"data:image/png;base64,{Convert.ToBase64String(ManagedItem.Image)}";
                }

                if (!string.IsNullOrWhiteSpace(ManagedItem?.ImageUrl))
                {
                    return $"{ApiBaseUrl}{ManagedItem.ImageUrl}?{Guid.NewGuid()}";
                }

                return $"data:image/gif;base64,{ManagedItemDefaultImages.Default}";
            }
        }

        protected async Task OnImageChange(InputFileChangeEventArgs args)
        {
            const string format = "image/png";
            var resizedImageFile = await args.File.RequestImageFileAsync(format, 150, 150);

            var buffer = new byte[resizedImageFile.Size];
            await resizedImageFile.OpenReadStream().ReadAsync(buffer);

            ManagedItem.Image = buffer;
        }
    }
}