using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Hydra.Module.Video.Models
{
    public class VideoClass
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(100, ErrorMessage = "Class Name is not valid.", MinimumLength = 3)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Class description is required.")]
        public string Description { get; set; }

        public int Id { get; set; }

        public byte[] Image { get; set; }

        public string ImageUrl { get; set; }

        [JsonPropertyName("groups")]
        public List<VideoGroup> VideoGroups { get; set; } = new();
    }
}