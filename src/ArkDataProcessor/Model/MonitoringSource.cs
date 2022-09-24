#nullable disable

using ArkDataProcessor.ArkGameModel;

namespace ArkDataProcessor
{
    public class MonitoringSource
    {
        public string ExternalConfiguration { get; set; }
        public string FilePath { get; set; }
        public List<UploadTarget> UploadTargets { get; set; }
        public List<Filter> Filters { get; set; }
        public MapDefinition MapDefinition { get; set; }
    }
}
