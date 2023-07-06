using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Anthology.Models
{
    /**
 * Effect Class
 * Contains the delta change in the momtive of an agent implementing the action.
 * One effect per motive type.
 * eg. Sleep action may affect the physical motive
 */
    public class Effect
    {
        /**
         * describes th emotive affected by this effected.
         * eg. if an action affects the social motive of an agent,
         * then motive = MotiveEnum.SOCIAL
         */
        public string Type { get; set; } = string.Empty;

        /** valence of effect on the motive */
        public float Delta { get; set; }
    }

    /**
     * Target types
     * List of values that determine who the target effects of an action (if applicable), are applied to.
     * This roughly mirrors the parameters of the people requirement
     */
    public static class TargetType
    {
        /**
         * all agents present when the action is started (ie at the action's associated location when the
         * agent begins to perform it) receive the target effects
         * This target type could be used for an agent making a public speech
         */
        public const string ALL = "all";

        /**
         * only agents who are present when the action is started (ie at the action's associated location 
         * when the agent begins to perform it), and who fit the SpecificPeoplePresent criteria of the people
         * requirement receive the target effects.
         * This is used for actions that only apply to a certain few people.
         */
        public const string SPECIFIC = "specific";

        /**
         * a single, random agent who is present when the action is started (ie at the action's associated 
         * location when the agent begins to perform it), and who fits the SpecificPeoplePresent criteria of the
         * people requirement receives the target effects
         * This could be used for an action that has one agent chat with a single friend out of a few specific options
         */
        public const string SPECIFIC_SINGLE = "specific_single";

        /**
         * a single, random agent who is present when the action is started (ie at the action's associated 
         * location when the agent begins to perform it) receives the target effects
         * This can be used for an action such as asking a stranger for directions
         */
        public const string RANDOM_PRESENT = "random_present";
    }

    /**
     * Action class all actions should inherit from
     * All actions have at least a name, requirements, and minimum time taken
     */
    public abstract class Action
    {
        /** Name of the action */
        public string Name { get; set; } = string.Empty;

        /** The minimum amount of time an action takes to execute */
        public int MinTime { get; set; }

        /** Optional flag to be set if this action cannot be selected by agents normally */
        public bool Hidden { get; set; } = false;

        /** Container of preconditions or requirements that must be fulfilled for this action to execute */
        public RequirementContainer Requirements { get; set; } = new();

        /** Filter through the set of requirements of this action to find the set of the given requirement type */
        public HashSet<Requirement> GetRequirementsByType(string type)
        {
            bool MismatchesType(Requirement req)
            {
                return req.ReqType != type;
            }

            HashSet<Requirement> reqs = new();
            reqs.UnionWith(Requirements.GetAll());
            reqs.RemoveWhere(MismatchesType);

            return reqs;
        }
    }

    /**
     * Action or Behavior to be executed by an agent
     * ex. sleep
     */
    public class PrimaryAction : Action
    {
        /** List of resulting changes to the motives of the agent that occur after this action is executed */
        [JsonPropertyOrder(1)]
        public Dictionary<string, float> Effects { get; set; } = new Dictionary<string, float>();
    }

    /**
     * Action or Behavior to be executed by an agent
     * ex. go to dinner
     */
    public class ScheduleAction : Action
    {
        /** Optional flag to be set if this action is performed immediately rather than scheduled for later */
        public bool Interrupt = false;

        /** Primary action that will be performed by the instigator of this action */
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string InstigatorAction { get; set; } = string.Empty;

        /** Primary action that will be performed by the target of this action */
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string TargetAction { get; set; } = string.Empty;

        /** 
         * The method of choosing which agent(s) will be the target of this action
         * NOTE: some target methods require the action to have a people requirement to function properly
         */
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Target { get; set; } = string.Empty;
    }

    /** 
     * Container class for sets of both schedule and primary actions.
     * This is primarily for file I/O with JSON serialization, as .NET 6
     * does not support polymorphic serialization :(
     */
    public class ActionContainer
    {
        /** Set of the schedule actions in the container */
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public HashSet<ScheduleAction> ScheduleActions { get; set; } = new();

        /** Set of the primary actions in the container */
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public HashSet<PrimaryAction> PrimaryActions { get; set; } = new();

        /** used to add actions to their appropriate sets */
        public void AddAction(Action action)
        {
            if (action is PrimaryAction p) { PrimaryActions.Add(p); }
            else if (action is ScheduleAction s) { ScheduleActions.Add(s); }
        }
    }
}
