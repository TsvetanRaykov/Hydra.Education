﻿namespace Hydra.Module.Video.Backend.Data
{
    public class UserToGroup
    {
        public string UserId { get; set; }

        public int GroupId { get; set; }
        public virtual VideoGroup VideoGroup { get; set; }
    }
}