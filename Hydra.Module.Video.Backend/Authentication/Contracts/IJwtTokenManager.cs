namespace Hydra.Module.Video.Backend.Authentication.Contracts
{
    public interface IJwtTokenManager
    {
        string Authenticate(string clientName, string clientSecret);
    }
}