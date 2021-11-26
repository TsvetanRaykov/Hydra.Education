using Microsoft.Extensions.DependencyInjection;

namespace Hydra.Module.Video.Backend.Contracts
{
    public interface IAddHydraModuleAuthentication
    {
        void AddHydraModuleJwtTokenAuthentication(IServiceCollection services);
    }
}