using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Hydra.Module.Video.Models
{
    public class VideoClass : IManagedItem
    {
        [Display(Name = "Class Name")]
        public string Name { get; set; }
        [Display(Name = "Class Description")]
        public string Description { get; set; }
        public int Id { get; set; }

        [Display(Name = "Class Image")]
        public byte[] Image { get; set; }
        [Display(Name = "Class Image")]
        public string ImageUrl { get; set; }

        [JsonPropertyName("groups")]
        public List<VideoGroup> VideoGroups { get; set; } = new();
    }
}