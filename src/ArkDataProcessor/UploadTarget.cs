#nullable disable

namespace ArkDataProcessor
{
    public class UploadTarget
    {
        public string Id { get; set; }
        public string Scheme { get; set; }
        public string RemoteTarget { get; set; }
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
