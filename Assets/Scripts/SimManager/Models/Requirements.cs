using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Anthology.Models
{
    /** Binary operation strings used for requirement threshold relationships */
    public static class BinOps
    {
        /** the string name of = relationships */
        public const string EQUALS = "=";

        /** the string name of > relationships */
        public const string GREATER = ">";

        /** the string name of < relationships */
        public const string LESS = "<";

        /** the string name of >= relationships */
        public const string GREATER_EQUALS = ">=";

        /** the string name of <= relationships */
        public const string LESS_EQUALS = "<=";
    }

    /** Requirement class all requirement types should inherit from */
    public abstract class Requirement
    {
        /** all requirements should have an enumerated type (or a string for file IO?) */
        public abstract string ReqType { get; }

        /** the string name of location type requirements */
        public const string LOCATION = "location";

        /** the string name of people type requirements */
        public const string PEOPLE = "people";

        /** the string name of motive type requirements */
        public const string MOTIVE = "motive";
    }

    /** 
     * Location Requirement
     * Requirements on the type of location the action takes place in.
     * Based on a tags system for locations.
     * eg: HasAllOf: { "restaurant" } implies to execute the action, one must be at a restaurant.
     */
    public class RLocation : Requirement
    {
        [JsonIgnore]
        public override string ReqType
        {
            get { return LOCATION; }
        }

        /** set of string tags that must be a subset of the location's tags for the action to occur */
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public HashSet<string> HasAllOf { get; set; } = new HashSet<string>();

        /** set of string tags in which their must exist at least one match with the location's tags for the action to occur */
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public HashSet<string> HasOneOrMoreOf { get; set; } = new HashSet<string>();

        /** set of string tags that must be a disjoint set of the location's tags for the action to occur */
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public HashSet<string> HasNoneOf { get; set; } = new HashSet<string>();
    }

    /**
     * People Requirement
     * Requirements on who is or must be present for an action.
     * eg. MinNumPeople = 2 implies that at least 2 people must be present for the action to occur
     */
    public class RPeople : Requirement
    {
        [JsonIgnore]
        public override string ReqType
        {
            get { return PEOPLE; }
        }

        /** the minimum number of people that must be present for the action to occur */
        public short MinNumPeople { get; set; }

        /** the maximum number of people that may be present for the action to occur */
        public short MaxNumPeople { get; set; }

        /** 
         * set of agents that must be present for the action to be completed.
         * eg. A cook must be present at a restaurant for food to be served to a customer.
         */
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public HashSet<string> SpecificPeoplePresent { get; set; } = new HashSet<string>();

        /** set of agents that must be absent for the action to be completed. */
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public HashSet<string> SpecificPeopleAbsent { get; set; } = new HashSet<string>();

        /**
         * Relationships that must exist between participating agents in order for the action to execute
         * eg. [teacher, student] relationships for the action "submit_homework"
         */
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public HashSet<string> RelationshipsPresent { get; set; } = new HashSet<string>();

        /**
         * Relationships that must not exist between participating agents in order for the action to execute
         * eg. [siblings] relationship for the action "kiss_romantically"
         */
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public HashSet<string> RelationshipsAbsent { get; set; } = new HashSet<string>();
    }

    /**
     * Motive Requirement
     * Requirements on the executing agent's current motive scores
     * eg. motive:"financial" > 2 implies the agent must have at least 2 financial score to execute the action
     */
    public class RMotive : Requirement
    {
        [JsonIgnore]
        public override string ReqType
        {
            get { return MOTIVE; }
        }

        /** 
         * describes the type of motive that must be tested for this requirement 
         * eg. Emotional, Social, Financial, etc.
         */
        public string MotiveType { get; set; } = string.Empty;

        /** Binary Operation used to test condition */
        public string Operation { get; set; } = string.Empty;

        /** Threshold value for testing the condition */
        public float Threshold { get; set; }
    }

    /** 
     * Container class for action requirements.
     * This is primarily for file I/O with JSON serialization, as .NET 6
     * does not support polymorphic serialization :(
     */
    public class RequirementContainer
    {
        /** Location requirements in the container */
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("Location Requirements")]
        public HashSet<RLocation>? Locations { get; set; }

        /** People requirements in the container */
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("People Requirements")]
        public HashSet<RPeople>? People { get; set; }

        /** Motive requirements in the container */
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("Motive Requirements")]
        public HashSet<RMotive>? Motives { get; set; }

        /** Add an arbitrary requirement to the container */
        public void AddRequirement(Requirement req)
        {
            if (req is RLocation rl)
            {
                Locations ??= new HashSet<RLocation>();
                Locations.Add(rl);
            }
            else if (req is RPeople rp)
            {
                People ??= new HashSet<RPeople>();
                People.Add(rp);
            }
            else if (req is RMotive rm)
            {
                Motives ??= new HashSet<RMotive>();
                Motives.Add(rm);
            }
        }

        /** Get a set of all requirements in the container */
        public HashSet<Requirement> GetAll()
        {
            HashSet<Requirement> reqs = new();
            if (Locations != null) reqs.UnionWith(Locations);
            if (People != null) reqs.UnionWith(People);
            if (Motives != null) reqs.UnionWith(Motives);
            return reqs;
        }
    }
}
