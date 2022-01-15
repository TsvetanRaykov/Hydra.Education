using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Hydra.Module.Video.Contracts;

namespace Hydra.Module.Video.Models
{
    public class VideoPlaylist : IManagedItem, IEquatable<VideoPlaylist>
    {
        [Display(Name = "Playlist Name")]
        public string Name { get; set; }
        [Display(Name = "Playlist Description")]
        public string Description { get; set; }
        public int Id { get; set; }
        [Display(Name = "Playlist Image")]
        public byte[] Image { get; set; }
        [Display(Name = "Playlist Image")]
        public string ImageUrl { get; set; }

        public List<Video> Videos { get; set; } = new();

        public List<VideoGroup> VideoGroups { get; set; }

        public bool Equals(VideoPlaylist other)
        {
            return other != null && Id == other.Id;
        }

    }
}