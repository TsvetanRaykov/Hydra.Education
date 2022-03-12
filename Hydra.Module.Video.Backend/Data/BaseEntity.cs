using System.ComponentModel.DataAnnotations;

namespace Hydra.Module.Video.Backend.Data;

public abstract class BaseEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string Description { get; set; }
}