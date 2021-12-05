namespace Hydra.Module.Video.Backend.Services
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using System.Threading.Tasks;

    public abstract class ServiceBase
    {
        protected readonly ILogger Logger;

        protected ServiceBase(ILogger logger)
        {
            Logger = logger;
        }
        protected async Task<string> UpdateDbAsync(DbContext db)
        {
            try
            {
                await db.SaveChangesAsync();
                return null;
            }
            catch (DbUpdateException ex)
            {
                Logger.LogError(nameof(UpdateDbAsync), ex);
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return "Database operation failed.";
            }
        }
    }
}