namespace ArkDataProcessor.ArkGameModel
{
    /// <summary>
    /// Stat indices and count.
    /// </summary>
    internal static class Stats
    {
        /// <summary>
        /// Total count of all stats.
        /// </summary>
        public const int StatsCount = 12;

        public const int Health = 0;
        public const int Stamina = 1;
        public const int Torpidity = 2;
        public const int Oxygen = 3;
        public const int Food = 4;
        public const int Water = 5;
        public const int Temperature = 6;
        public const int Weight = 7;
        public const int MeleeDamageMultiplier = 8;
        public const int SpeedMultiplier = 9;
        public const int TemperatureFortitude = 10;
        public const int CraftingSpeedMultiplier = 11;

        /// <summary>
        /// Returns the stat-index for the given order index (like it is ordered in game).
        /// </summary>
        public static readonly int[] DisplayOrder = {
            Health,
            Stamina,
            Oxygen,
            Food,
            Water,
            Temperature,
            Weight,
            MeleeDamageMultiplier,
            SpeedMultiplier,
            TemperatureFortitude,
            CraftingSpeedMultiplier,
            Torpidity
        };
    }
}
