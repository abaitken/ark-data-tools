namespace ArkDataProcessor
{
    static class StringExtensions
    {
        public static string ThrowIfNullOrEmpty(this string? value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException();
            return value;
        }
    }
}
