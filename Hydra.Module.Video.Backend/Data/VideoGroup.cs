using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hydra.Module.Video.Backend.Data
{
    public class VideoGroup
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required]
        public int VideoClassId { get; set; }
        public VideoClass VideoClass { get; set; }

        public List<UserToGroup> Users { get; set; }
    }
}