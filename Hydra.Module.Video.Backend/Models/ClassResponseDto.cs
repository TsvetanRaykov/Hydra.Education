using System.Collections.Generic;

namespace Hydra.Module.Video.Backend.Models
{
    public class ClassResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TrainerId { get; set; }
        public string ImageUrl { get; set; }
        public List<GroupResponseDto> Groups { get; set; }
    }
}