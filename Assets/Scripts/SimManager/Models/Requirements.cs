using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Anthology.Models
{
    /// <summary>
    /// Binary operation strings used for requirement threshold relationships.
    /// </summary>
    public static class BinOps
    {
        /// <summary>
        /// The string name of equal relationships.
        /// </summary>
        public const string EQUALS = "=";

        /// <summary>
        /// The string name of greater-than relationships.
        /// </summary>
        public const string GREATER = ">";

        /// <summary>
        /// The string name of less-than relationships.
        /// </summary>
        public const string LESS = "<";

        /// <summary>
        /// The string name of greater-than-or-equal-to relationships.
        /// </summary>
        public const string GREATER_EQUALS = ">=";

        /// <summary>
        /// The string name of less-than-or-equal-to relationships.
        /// </summary>
        public const string LESS_EQUALS = "<=";
    }

    /// <summary>
    /// Requirement class all requirement types should inherit from.
    /// </summary>
    public abstract class Requirement
    {
        /// <summary>
        /// All requirements should have an enumerated type (or a string for file IO?).
        /// </summary>
        public abstract string ReqType { get; }

        /// <summary>
        /// The string name of location type requirements
        /// </summary>
        public const string LOCATION = "location";

        /// <summary>
        /// The string name of people type requirements.
        /// </summary>
        public const string PEOPLE = "people";

        /// <summary>
        /// The string name of motive type requirements.
        /// </summary>
        public const string MOTIVE = "motive";
    }

    /// <summary>
    /// Location Requirement.
    /// Requirements on the type of location the action takes place in.
    /// Based on a tags system for locations.
    /// eg: HasAllOf: { "restaurant" } implies to execute the action, one must be at a restaurant.
    /// </summary>
    public class RLocation : Requirement
    {
        /// <summary>
        /// Returns the required type "location".
        /// </summary>
        [JsonIgnore]
        public override string ReqType
        {
            get { return LOCATION; }
        }

        /// <summary>
        /// Set of string tags that must be a subset of the location's tags for the action to occur.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<string> HasAllOf { get; set; } = new();

        /// <summary>
        /// Set of string tags in which their must exist at least one match with the location's tags for the action to occur.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<string> HasOneOrMoreOf { get; set; } = new();

        /// <summary>
        /// Set of string tags that must be a disjoint set of the location's tags for the action to occur.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<string> HasNoneOf { get; set; } = new();
    }

    /// <summary>
    /// People Requirement,
    /// Requirements on who is or must be present for an action.
    /// eg. MinNumPeople = 2 implies that at least 2 people must be present for the action to occur.
    /// </summary>
    public class RPeople : Requirement
    {
        /// <summary>
        /// Returns the required type "people".
        /// </summary>
        [JsonIgnore]
        public override string ReqType
        {
            get { return PEOPLE; }
        }

        /// <summary>
        /// The minimum number of people that must be present for the action to occur.
        /// </summary>
        public short MinNumPeople { get; set; }

        /// <summary>
        /// The maximum number of people that may be present for the action to occur.
        /// </summary>
        public short MaxNumPeople { get; set; }

        /// <summary>
        /// Set of agents that must be present for the action to be completed.
        /// eg. A cook must be present at a restaurant for food to be served to a customer.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<string> SpecificPeoplePresent { get; set; } = new();

        /// <summary>
        /// Set of agents that must be absent for the action to be completed.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<string> SpecificPeopleAbsent { get; set; } = new();

        /// <summary>
        /// Relationships that must exist between participating agents in order for the action to execute.
        /// eg. [teacher, student] relationships for the action "submit_homework".
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<string> RelationshipsPresent { get; set; } = new();

        /// <summary>
        /// Relationships that must not exist between participating agents in order for the action to execute.
        /// eg. [siblings] relationship for the action "kiss_romantically".
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<string> RelationshipsAbsent { get; set; } = new();
    }

    /// <summary>
    /// Motive Requirement.
    /// Requirements on the executing agent's current motive scores.
    /// eg. motive:"financial" > 2 implies the agent must have at least 2 financial score to execute the action.
    /// </summary>
    public class RMotive : Requirement
    {
        /// <summary>
        /// Returns the required type "motive".
        /// </summary>
        [JsonIgnore]
        public override string ReqType
        {
            get { return MOTIVE; }
        }

        /// <summary>
        /// Describes the type of motive that must be tested for this requirement.
        /// eg. Emotional, Social, Financial, etc.
        /// </summary>
        public string MotiveType { get; set; } = string.Empty;

        /// <summary>
        /// Binary Operation used to test condition.
        /// </summary>
        public string Operation { get; set; } = string.Empty;

        /// <summary>
        /// Threshold value for testing the condition.
        /// </summary>
        public float Threshold { get; set; }
    }

    /// <summary>
    /// Container class for action requirements.
    /// This is primarily for file I/O with JSON serialization, as .NET 6
    /// does not support polymorphic serialization. :(
    /// </summary>
    public class RequirementContainer
    {
        /// <summary>
        /// Location requirements in the container.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("Location Requirements")]
        public List<RLocation>? Locations { get; set; }

        /// <summary>
        /// People requirements in the container.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("People Requirements")]
        public List<RPeople>? People { get; set; }

        /// <summary>
        /// Motive requirements in the container.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("Motive Requirements")]
        public List<RMotive>? Motives { get; set; }

        /// <summary>
        /// Add an arbitrary requirement to the container.
        /// </summary>
        /// <param name="req">The requirement to add.</param>
        public void AddRequirement(Requirement req)
        {
            if (req is RLocation rl)
            {
                Locations ??= new();
                Locations.Add(rl);
            }
            else if (req is RPeople rp)
            {
                People ??= new();
                People.Add(rp);
            }
            else if (req is RMotive rm)
            {
                Motives ??= new();
                Motives.Add(rm);
            }
        }

        /// <summary>
        /// Get a set of all requirements in the container.
        /// </summary>
        /// <returns>All of container's requirements as a set.</returns>
        public IEnumerable<Requirement> GetAll()
        {
            List<Requirement> reqs = new();
            if (Locations != null) reqs.AddRange(Locations);
            if (People != null) reqs.AddRange(People);
            if (Motives != null) reqs.AddRange(Motives);
            return reqs;
        }
    }
}
