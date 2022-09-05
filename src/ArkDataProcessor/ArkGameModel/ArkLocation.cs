using SavegameToolkit.Types;

namespace ArkDataProcessor.ArkGameModel
{
    public class ArkLocation
    {
        public ArkLocation()
        {
        }

        public ArkLocation(LocationData locationData) : this()
        {
            X = locationData.X;
            Y = locationData.Y;
            Z = locationData.Z;
        }

        public ArkLocation(LocationData locationData, MapDefinition mapDefinition) : this(locationData)
        {
            MapName = mapDefinition.Name;
            Latitude = locationData.Y / mapDefinition.LatitudeDivisor + mapDefinition.LatitudeShift;
            Longitude = locationData.X / mapDefinition.LongitudeDivisor + mapDefinition.LongitudeShift;
            // TODO : Height? Set sea level?
        }

        public string? MapName { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
    }
}
