#nullable disable

namespace ArkDataProcessor
{
    public class Filter
    {
        public string Id { get; set; }
        public List<string> Include { get; set; }
        public List<string> Exclude { get; set; }
    }
}
