using Newtonsoft.Json;

namespace ArkDataProcessor.ArkGameModel
{
    /// <summary>
    /// Used to translate unreal engine world coordinates (x, y) into gps coordinates (latitude and longitude) and gps coordinates into pixel offsets on individual map images.
    /// </summary>
    /// <example>
    /// X = (longitude - longitude shift) * longitude divisor
    /// Y = (latitude - latitude shift) * latitude divisor
    /// Longitude = x / longitude divisor + longitude shift
    /// Latitude = y / latitude divisor + latitude shift
    /// </example>
    /// <remarks>
    /// Available in the devkit, from map authors, codebehind for wiki resource maps
    /// </remarks>
    public class MapDefinition
    {
        [JsonConstructor]
        public MapDefinition(
            float latitudeShift,
            float latitudeDivisor,
            float longitudeShift,
            float longitudeDivisor)
        {
            LatitudeShift = latitudeShift;
            LatitudeDivisor = latitudeDivisor;
            LongitudeShift = longitudeShift;
            LongitudeDivisor = longitudeDivisor;
        }

        public string Name { get; set; }

        /// <summary>
        /// Latitude shift
        /// </summary>
        public float LatitudeShift { get; set; }
        /// <summary>
        /// Latitude divisor
        /// </summary>
        public float LatitudeDivisor { get; set; }
        /// <summary>
        /// Longitude shift
        /// </summary>
        public float LongitudeShift { get; set; }
        /// <summary>
        /// Longitude divisor
        /// </summary>
        public float LongitudeDivisor { get; set; }
    }
}
