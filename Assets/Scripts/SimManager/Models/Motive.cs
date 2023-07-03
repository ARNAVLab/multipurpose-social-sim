namespace Anthology.Models
{
    /** Motive class all motive types should inherit from */
    public static class Motive
    {
        /** the maximum value of a motive */
        public const float MAX = 5f;

        /** the minimum value of a motive */
        public const float MIN = 1f;

        /** the string name of physical type motives */
        public const string PHYSICAL = "physical";

        /** the string name of emotional type motives */
        public const string EMOTIONAL = "emotional";

        /** the string name of social type motives */
        public const string SOCIAL = "social";

        /** the string name of financial type motives */
        public const string FINANCIAL = "financial";

        /** the string name of accomplishment type motives */
        public const string ACCOMPLISHMENT = "accomplishment";
    }
}