using System.ComponentModel.DataAnnotations;

namespace Hydra.Module.Video.Models
{
    public class VideoGroup
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(100, ErrorMessage = "Group Name is not valid.", MinimumLength = 3)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "A short description of the Group is required.")]
        public string Description { get; set; }

        public int Id { get; set; }

        public byte[] Image { get; set; }

        public string ImageUrl { get; set; }

        public int ClassId { get; set; }
    }
}