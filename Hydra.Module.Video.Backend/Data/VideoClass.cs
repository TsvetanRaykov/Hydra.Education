using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hydra.Module.Video.Backend.Data
{
    public class VideoClass
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string TrainerId { get; set; }
        
        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public ICollection<VideoGroup> VideoGroups { get; set; }

    }
}