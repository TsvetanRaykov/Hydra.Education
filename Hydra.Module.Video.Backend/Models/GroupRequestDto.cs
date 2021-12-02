namespace Hydra.Module.Video.Backend.Models
{
    public class GroupRequestDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }
        public int ClassId { get; set; }
    }
}