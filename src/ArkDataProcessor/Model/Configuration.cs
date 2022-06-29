#nullable disable

namespace ArkDataProcessor
{
    public class Configuration
    {
        public List<MonitoringSource> MonitoringSources { get; set; }
        public uint LongDelay { get; set; }
        public uint ShortDelay { get; set; }
        public List<SharedSetting> SharedSettings { get; set; }
    }
}
