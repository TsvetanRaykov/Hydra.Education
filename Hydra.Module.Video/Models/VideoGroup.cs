using System.ComponentModel.DataAnnotations;

namespace Hydra.Module.Video.Models
{
    public class VideoGroup : IManagedItem
    {
        [Display(Name = "Group Name")]
        public string Name { get; set; }

        [Display(Name = "Group Description")]
        public string Description { get; set; }

        public int Id { get; set; }

        [Display(Name = "Group Image")]
        public byte[] Image { get; set; }

        [Display(Name = "Group Image")]
        public string ImageUrl { get; set; }

        public int ClassId { get; set; }
    }
}