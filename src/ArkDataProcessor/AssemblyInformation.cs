using System.Reflection;

namespace ArkDataProcessor
{
    /// <summary>
    /// Returns information about the given assembly
    /// </summary>
    public class AssemblyInformation
    {
        private readonly Assembly _assembly;

        /// <summary>
        /// Constructs this object
        /// </summary>
        /// <param name="assembly">Assembly to gather information from</param>
        public AssemblyInformation(Assembly assembly)
        {
            _assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
        }

        public string Title => Get<AssemblyTitleAttribute>(i => i.Title);
        public string Company => Get<AssemblyCompanyAttribute>(i => i.Company);
        public string Description => Get<AssemblyDescriptionAttribute>(i => i.Description);
        public string Product => Get<AssemblyProductAttribute>(i => i.Product);
        public string Trademark => Get<AssemblyTrademarkAttribute>(i => i.Trademark);
        public string Configuration => Get<AssemblyConfigurationAttribute>(i => i.Configuration);
        public string FileVersion => Get<AssemblyFileVersionAttribute>(i => i.Version);
        public string Copyright => Get<AssemblyCopyrightAttribute>(i => i.Copyright);
        public string InformationalVersion => Get<AssemblyInformationalVersionAttribute>(i => i.InformationalVersion);
        public string AssemblyVersion => Get<AssemblyVersionAttribute>(i => i.Version);

        private string Get<T>(Func<T, string> selector)
            where T : Attribute
        {
            var attributes = _assembly.GetCustomAttributes<T>().ToList();

            if (attributes.Count == 0)
                return string.Empty;

            return selector(attributes[0]);
        }
    }
}
