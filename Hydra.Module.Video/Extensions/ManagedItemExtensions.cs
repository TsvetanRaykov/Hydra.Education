namespace Hydra.Module.Video.Extensions
{
    using Models;
    using System.ComponentModel.DataAnnotations;
    public static class ManagedItemExtensions
    {
        public static string GetDisplayName(this IManagedItem objectItem, string propertyName)
        {
            var propInfo = objectItem.GetType().GetProperty(propertyName);
            var displayNameAttribute = propInfo?.GetCustomAttributes(typeof(DisplayAttribute), false);
            return (displayNameAttribute?[0] as DisplayAttribute)?.Name;
        }
    }
}