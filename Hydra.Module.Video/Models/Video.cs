using System;
using System.Collections.Generic;

namespace Hydra.Module.Video.Models
{
    public class Video : IEquatable<Video>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime UploadedOn { get; set; }
        public string UploadedBy { get; set; }
        public string Url { get; set; }

        public List<VideoPlaylist> PlayLists { get; set; }

        public bool Equals(Video other)
        {
            return Id == other?.Id;
        }

    }
}