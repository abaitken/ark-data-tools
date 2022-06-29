using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable disable
namespace ArkDataProcessor.Extensions
{
    internal static class SharedSettingsExtensions
    {
        public static T GetValueOrDefault<T>(this IEnumerable<SharedSetting> settings, string key)
        {
            var setting = settings.FirstOrDefault(o => o.Key.Equals(key));
            if (setting == null)
                return default;

            var value = setting.Value;
            return (T)value;
        }
    }
}
