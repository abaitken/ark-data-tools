namespace ArkDataProcessor
{
    static class TemporaryFileServices
    {
        public static string GenerateFileName(string ext = ".tmp")
        {
            return Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}{ext}");
        }
    }
}
