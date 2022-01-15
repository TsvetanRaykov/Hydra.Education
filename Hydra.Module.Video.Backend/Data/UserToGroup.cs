namespace Hydra.Module.Video.Backend.Data
{
    public class UserToGroup
    {
        public string UserId { get; set; }

        public int GroupId { get; set; }
        public VideoGroup Group { get; set; }
    }
}