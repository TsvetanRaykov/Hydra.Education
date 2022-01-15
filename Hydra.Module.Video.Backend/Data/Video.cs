using System;
using System.Collections.Generic;

namespace Hydra.Module.Video.Backend.Data
{
    public class Video
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime UploadedOn { get; set; }
        public string UploadedBy { get; set; }
        public string Url { get; set; }
        public ICollection<VideoToPlaylist> Playlists { get; set; }
    }
}