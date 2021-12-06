using System.ComponentModel.DataAnnotations;

namespace Hydra.Module.Video.Models
{
    public class VideoPlaylist : IManagedItem
    {
        [Display(Name = "Playlist Name")]
        public string Name { get; set; }
        [Display(Name = "Playlist Description")]
        public string Description { get; set; }
        public int Id { get; set; }
        [Display(Name = "Playlist Image")]
        public byte[] Image { get; set; }
        [Display(Name = "Playlist Image")]
        public string ImageUrl { get; set; }
    }
}