using System.Threading.Tasks;

namespace Hydra.Module.Video.Contracts
{
    public interface ITokenService
    {
        Task<string> GetTokenAsync();
    }
}