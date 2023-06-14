namespace Anthology.Models
{
    /** Motive class all motive types should inherit from */
    public abstract class Motive
    {
        /** all motives should have a type */
        public abstract string Type { get; }

        /** all motives should have a numeric amount */
        public float Amount { get; set; }

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

    public class MPhysical : Motive
    {
        public override string Type
        {
            get { return PHYSICAL; }
        }
    }

    public class MEmotional : Motive
    {
        public override string Type
        {
            get { return EMOTIONAL; }
        }
    }

    public class MSocial : Motive
    {
        public override string Type
        {
            get { return SOCIAL; }
        }
    }

    public class MFinancial : Motive
    {
        public override string Type
        {
            get { return FINANCIAL; }
        }
    }

    public class MAccomplishment : Motive
    {
        public override string Type
        {
            get { return ACCOMPLISHMENT; }
        }
    }
}
