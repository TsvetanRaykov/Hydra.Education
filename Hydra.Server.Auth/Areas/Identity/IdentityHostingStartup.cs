using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Hydra.Server.Auth.Areas.Identity.IdentityHostingStartup))]
namespace Hydra.Server.Auth.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}