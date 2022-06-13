#nullable disable

namespace ArkDataProcessor
{
    public class MonitoringSource
    {
        public string FilePath { get; set; }
        public List<UploadTarget> UploadTargets { get; set; }
        public List<Filter> Filters { get; set; }
    }
}
