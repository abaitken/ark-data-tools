#nullable disable

namespace ArkDataProcessor
{
    public class Configuration
    {
        public List<UploadTarget> UploadTargets { get; set; }
        public uint LongDelay { get; set; }
        public uint ShortDelay { get; set; }
        public string SaveFilePath { get; set; }
    }
}
