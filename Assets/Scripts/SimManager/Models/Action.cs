using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Anthology.Models
{
    /// <summary>
    /// Contains the delta change in the momtive of an agent implementing the action.
    /// One effect per motive type, eg. sleep action may affect the physical motive.
    /// </summary>
    public class Effect
    {
        /// <summary>
        /// Describes th emotive affected by this effected,
        /// eg. if an action affects the social motive of an agent,
        /// then motive = MotiveEnum.SOCIAL.
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Valence of effect on the motive.
        /// </summary>
        public float Delta { get; set; }
    }

    /// <summary>
    /// List of values that determine who the target effects of an action (if applicable), are applied to.
    /// This roughly mirrors the parameters of the people requirement.
    /// </summary>
    public static class TargetType
    {
        /// <summary>
        /// All agents present when the action is started (ie. at the action's associated location when the
        /// agent begins to perform it) receive the target effects.
        /// This target type could be used for an agent making a public speech.
        /// </summary>
        public const string ALL = "all";

        /// <summary>
        /// Only agents who are present when the action is started (ie. at the action's associated location 
        /// when the agent begins to perform it) and who fit the SpecificPeoplePresent criteria of the people
        /// requirement receive the target effects.
        /// This is used for actions that only apply to a certain few people.
        /// </summary>
        public const string SPECIFIC = "specific";

        /// <summary>
        /// A single, random agent who is present when the action is started (ie. at the action's associated 
        /// location when the agent begins to perform it) and who fits the SpecificPeoplePresent criteria of the
        /// people requirement receives the target effects.
        /// This could be used for an action that has one agent chat with a single friend out of a few specific options.
        /// </summary>
        public const string SPECIFIC_SINGLE = "specific_single";

        /// <summary>
        /// A single, random agent who is present when the action is started (ie. at the action's associated
        /// location when the agent begins to perform it) receives the target effects.
        /// This can be used for an action such as asking a stranger for directions.
        /// </summary>
        public const string RANDOM_PRESENT = "random_present";
    }

    /// <summary>
    /// Action class all actions should inherit from.
    /// All actions have at least a name, requirements, and minimum time taken.
    /// </summary>
    public abstract class Action
    {
        /// <summary>
        /// Name of the action.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The minimum amount of time an action takes to execute.
        /// </summary>
        public int MinTime { get; set; }

        /// <summary>
        /// Optional flag to be set if this action cannot be selected by agents normally.
        /// </summary>
        public bool Hidden { get; set; } = false;

        /// <summary>
        /// Container of preconditions or requirements that must be fulfilled for this action to execute.
        /// </summary>
        public RequirementContainer Requirements { get; set; } = new();


        /// <summary>
        /// Filter through the set of requirements of this action to find the requirements of the given type.
        /// </summary>
        public List<Requirement> GetRequirementsByType(string type)
        {
            List<Requirement> reqs = new();
            IEnumerable<Requirement> allReqs = Requirements.GetAll();
            IEnumerator<Requirement> enumerator = allReqs.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current.ReqType == type)
                    reqs.Add(enumerator.Current);
            }
            return reqs;
        }
    }

    /// <summary>
    /// Action or behavior to be executed by an agent (ex. sleep).
    /// </summary>
    public class PrimaryAction : Action
    {
        /// <summary>
        /// List of resulting changes to the motives of the agent that occur after this action is executed.
        /// </summary>
        [JsonPropertyOrder(1)]
        public Dictionary<string, float> Effects { get; set; } = new Dictionary<string, float>();
    }

    /// <summary>
    /// Action or Behavior to be executed by an agent (ex. go to dinner).
    /// </summary>
    public class ScheduleAction : Action
    {
        /// <summary>
        /// Optional flag to be set if this action is performed immediately rather than scheduled for later.
        /// </summary>
        public bool Interrupt = false;

        /// <summary>
        /// Primary action that will be performed by the instigator of this action.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string InstigatorAction { get; set; } = string.Empty;

        /// <summary>
        /// Primary action that will be performed by the target of this action.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string TargetAction { get; set; } = string.Empty;

        /// <summary>
        /// The method of choosing which agent(s) will be the target of this action.
        /// NOTE: some target methods require the action to have a people requirement to function properly.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Target { get; set; } = string.Empty;
    }

    /// <summary>
    /// Container class for sets of both schedule and primary actions.
    /// This is primarily for file I/O with JSON serialization, as .NET 6
    /// does not support polymorphic serialization. :(
    /// </summary>
    public class ActionContainer
    {
        /// <summary>
        /// Set of the schedule actions in the container.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public HashSet<ScheduleAction> ScheduleActions { get; set; } = new();

        /// <summary>
        /// Set of the primary actions in the container.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public HashSet<PrimaryAction> PrimaryActions { get; set; } = new();

        /// <summary>
        /// Used to add actions to their appropriate sets.
        /// </summary>
        /// <param name="action">The action to be added.</param>
        public void AddAction(Action action)
        {
            if (action is PrimaryAction p) { PrimaryActions.Add(p); }
            else if (action is ScheduleAction s) { ScheduleActions.Add(s); }
        }
    }
}
