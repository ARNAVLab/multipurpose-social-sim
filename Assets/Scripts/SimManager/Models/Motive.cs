namespace Anthology.Models
{
    /// <summary>
    /// Motive class all motive types should inherit from.
    /// </summary>
    public static class Motive
    {
        /// <summary>
        /// The maximum value of a motive.
        /// </summary>
        public const float MAX = 5f;

        /// <summary>
        /// The minimum value of a motive.
        /// </summary>
        public const float MIN = 1f;

        /// <summary>
        /// The string name of physical type motives.
        /// </summary>
        public const string PHYSICAL = "physical";

        /// <summary>
        /// The string name of emotional type motives.
        /// </summary>
        public const string EMOTIONAL = "emotional";

        /// <summary>
        /// The string name of social type motives.
        /// </summary>
        public const string SOCIAL = "social";

        /// <summary>
        /// The string name of financial type motives.
        /// </summary>
        public const string FINANCIAL = "financial";

        /// <summary>
        /// The string name of accomplishment type motives.
        /// </summary>
        public const string ACCOMPLISHMENT = "accomplishment";
    }
}