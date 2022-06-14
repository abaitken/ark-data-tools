using System.Reflection;

namespace ArkDataProcessor
{
    internal sealed class EmbeddedResource
    {
        private readonly Assembly _assembly;
        private readonly string _resourceName;

        public EmbeddedResource(Assembly assembly, string resourceName)
        {
            _assembly = assembly;
            _resourceName = resourceName;
        }

        public Stream? Read()
        {
            return _assembly.GetManifestResourceStream(_resourceName);
        }
    }
}
