using Microsoft.Extensions.Logging;

namespace ArkDataProcessor
{
    internal class ConfigurationValidation
    {
        private readonly ILogger<ConfigurationValidation> _logger;

        public ConfigurationValidation(ILogger<ConfigurationValidation> logger)
        {
            _logger = logger;
        }

        public void Validate(Configuration configuration)
        {
            if (configuration.LongDelay == 0)
                throw new InvalidOperationException($"{nameof(Configuration.LongDelay)} must be a value greater than 0");

            if (configuration.ShortDelay == 0)
                throw new InvalidOperationException($"{nameof(Configuration.ShortDelay)} must be a value greater than 0");

            if(string.IsNullOrWhiteSpace(configuration.SaveFilePath))
                throw new InvalidOperationException($"{nameof(Configuration.SaveFilePath)} must have a value");

            if(configuration.UploadTargets.Count == 0)
                throw new InvalidOperationException($"No {nameof(Configuration.UploadTargets)} defined");

            foreach (var uploadTarget in configuration.UploadTargets)
            {
                ValidateUploadTarget(uploadTarget);
            }
        }

        private void ValidateUploadTarget(UploadTarget uploadTarget)
        {
            var validIds = new[] { "tamed_breeding_data", "tamed_creature_locations", "wild_creature_locations", "structure_locations" };
            var validSchemes = new[] { "copy", "sftp" };
            if (string.IsNullOrWhiteSpace(uploadTarget.Id))
                throw new InvalidOperationException($"Upload target Id is required");

            if (!validIds.Contains(uploadTarget.Id))
            {
                _logger.LogWarning($"'{uploadTarget.Id}' is not a valid upload target Id. Upload target will be disabled");
                return;
            }

            if (string.IsNullOrWhiteSpace(uploadTarget.Scheme))
                throw new InvalidOperationException($"Upload target Scheme is required");
            if (string.IsNullOrWhiteSpace(uploadTarget.RemoteTarget))
                throw new InvalidOperationException($"Upload target RemoteTarget is required");

            if (!validSchemes.Contains(uploadTarget.Scheme))
                throw new InvalidOperationException($"'{uploadTarget.Scheme}' is not a valid upload target Scheme");

            if(uploadTarget.Scheme.Equals("stfp"))
            {
                if (string.IsNullOrWhiteSpace(uploadTarget.Host))
                    throw new InvalidOperationException($"'{uploadTarget.Host}' is required");
                if (string.IsNullOrWhiteSpace(uploadTarget.Username))
                    throw new InvalidOperationException($"'{uploadTarget.Username}' is required");
                if (string.IsNullOrWhiteSpace(uploadTarget.Password))
                    throw new InvalidOperationException($"'{uploadTarget.Password}' is required");
            }
        }
    }
}