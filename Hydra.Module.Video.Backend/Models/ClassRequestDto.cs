namespace Hydra.Module.Video.Backend.Models
{
    public class ClassRequestDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }
        public string ImageUrl { get; set; }
    }
}