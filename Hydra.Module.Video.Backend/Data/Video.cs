using System;
using System.Collections.Generic;

namespace Hydra.Module.Video.Backend.Data
{
    public class Video : BaseEntity
    {
        public DateTime UploadedOn { get; set; }
        public string UploadedBy { get; set; }
        public string Url { get; set; }
        public ICollection<VideoToPlaylist> Playlists { get; set; }
    }
}