using System.Threading.Tasks;

namespace Hydra.Module.Video.Services.Contracts
{
    public interface ITokenService
    {
        Task<string> GetTokenAsync();
    }
}