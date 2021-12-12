namespace Hydra.Module.Video.Contracts
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