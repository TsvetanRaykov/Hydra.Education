using System.Collections.Generic;

namespace Hydra.Module.Video.Backend.Data
{
    public class VideoClass : BaseEntity
    {
        public string TrainerId { get; set; }
        public string ImageUrl { get; set; }
        public ICollection<VideoGroup> VideoGroups { get; set; }

    }
}