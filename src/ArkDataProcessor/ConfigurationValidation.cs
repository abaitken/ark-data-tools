using Microsoft.Extensions.Logging;

namespace ArkDataProcessor
{
    internal class ConfigurationValidation
    {
        private readonly ILogger<ConfigurationValidation> _logger;
        private readonly List<string> _validIds;

        public ConfigurationValidation(ILogger<ConfigurationValidation> logger)
        {
            _logger = logger;
            var factory = new DataProcessingPipelineFactory();
            _validIds = factory.CreatePipelines().Select(v => v.Id).ToList();
        }

        public void Validate(Configuration configuration)
        {
            if (configuration.LongDelay == 0)
                throw new InvalidOperationException($"{nameof(Configuration.LongDelay)} must be a value greater than 0");

            if (configuration.ShortDelay == 0)
                throw new InvalidOperationException($"{nameof(Configuration.ShortDelay)} must be a value greater than 0");

            if (configuration.MonitoringSources == null || configuration.MonitoringSources.Count == 0)
                throw new InvalidOperationException($"No {nameof(Configuration.MonitoringSources)} defined");

            foreach (var monitoringSource in configuration.MonitoringSources)
            {
                ValidateMonitoringSource(monitoringSource);
            }
        }

        private void ValidateMonitoringSource(MonitoringSource monitoringSource)
        {
            if (string.IsNullOrWhiteSpace(monitoringSource.FilePath))
                throw new InvalidOperationException($"{nameof(MonitoringSource.FilePath)} must have a value");

            if (monitoringSource.UploadTargets == null || monitoringSource.UploadTargets.Count == 0)
                throw new InvalidOperationException($"No {nameof(MonitoringSource.UploadTargets)} defined");

            foreach (var uploadTarget in monitoringSource.UploadTargets)
            {
                ValidateUploadTarget(uploadTarget);
            }

            if (monitoringSource.Filters != null)
            {
                foreach (var filter in monitoringSource.Filters)
                    ValidateFilter(filter);

                var grouped = from item in monitoringSource.Filters
                              group item by item.Id into g
                              select new
                              {
                                  g.Key,
                                  Count = g.Count()
                              };

                foreach (var item in grouped)
                {
                    if (item.Count <= 1)
                        continue;

                    _logger.LogWarning($"Multiple filters for '{item.Key}' found. Only the first filter will be used.");
                }
            }
        }

        private void ValidateFilter(Filter filter)
        {
            if (string.IsNullOrWhiteSpace(filter.Id))
                throw new InvalidOperationException($"Filter Id is required");

            if (!_validIds.Contains(filter.Id))
            {
                _logger.LogWarning($"'{filter.Id}' is not a valid upload target Id. Filter will be disabled");
                return;
            }
        }

        private void ValidateUploadTarget(UploadTarget uploadTarget)
        {
            var validSchemes = new[] { "copy", "sftp" };
            if (string.IsNullOrWhiteSpace(uploadTarget.Id))
                throw new InvalidOperationException($"Upload target Id is required");

            if (!_validIds.Contains(uploadTarget.Id))
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