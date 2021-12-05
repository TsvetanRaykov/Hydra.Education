using System;
using System.ComponentModel.DataAnnotations;

namespace Hydra.Module.Video.Models
{
    public interface IManagedItem
    {
        string Name { get; set; }
        string Description { get; set; }
        int Id { get; set; }
        byte[] Image { get; set; }
        string ImageUrl { get; set; }
    }
}