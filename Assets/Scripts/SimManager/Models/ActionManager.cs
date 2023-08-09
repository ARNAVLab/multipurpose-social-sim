using System;
using System.Collections.Generic;

namespace Anthology.Models
{
    /// <summary>
    /// Manages a container of actions throughout the lifetime of the simulation.
    /// </summary>
    public static class ActionManager
    {
        /// <summary>
        /// Actions available in the simulation.
        /// </summary>
        public static ActionContainer Actions { get; set; } = new();

        /// <summary>
        /// Compiled list of all actions of both scheduled and primary actions.
        /// </summary>
        public static List<Action> AllActions { get; set; } = new();

        /// <summary>
        /// Initializes/resets all action manager variables.
        /// </summary>
        /// <param name="path">Path of actions JSON file.</param>
        public static void Init(string path)
        {
            Actions.ScheduleActions.Clear();
            Actions.PrimaryActions.Clear();
            AllActions.Clear();
            World.ReadWrite.LoadActionsFromFile(path);

            foreach (Action action in Actions.ScheduleActions)
            {
                AllActions.Add(action);
            }
            foreach (Action action in Actions.PrimaryActions)
            {
                AllActions.Add(action);
            }
        }

        /// <summary>
        /// Clears all actions from the system.
        /// </summary>
        public static void Reset()
        {
            Actions.ScheduleActions.Clear();
            Actions.PrimaryActions.Clear();
            AllActions.Clear();
        }

        /// <summary>
        /// Adds the given action to both action structures.
        /// </summary>
        /// <param name="action">The action to add.</param>
        public static void AddAction(Action action)
        {
            Actions.AddAction(action);
            AllActions.Add(action);
        }

        /// <summary>
        /// Retrieves an action with the specified name from the set of actions available in the simulation.
        /// </summary>
        /// <param name="actionName">The name of the action to find.</param>
        /// <returns>The action with specified name.</returns>
        /// <exception cref="Exception">Thrown when action cannot be found.</exception>
        public static Action GetActionByName(string actionName)
        {
            bool HasName(Action action)
            {
                return action.Name == actionName;
            }
            Action? action = AllActions.Find(HasName);
            return action ?? throw new Exception("Action with name: " + actionName + " cannot be found.");
        }

        /// <summary>
        /// Returns the net effect for an action for a specific agent.
        /// Takes into account the agent's current motivation statuses.
        /// </summary>
        /// <param name="agent">The agent relevant to retrieve motives from.</param>
        /// <param name="action">The action to calculate net effect.</param>
        /// <returns>How much to affect motive by.</returns>
        public static float GetEffectDeltaForAgentAction(Agent agent, Action action)
        {
            float deltaUtility = 0f;

            if (action is PrimaryAction pAction)
            {
                foreach (KeyValuePair<string, float> e in pAction.Effects)
                {
                    float delta = e.Value;
                    float current = agent.Motives[e.Key];
                    deltaUtility += Math.Clamp(delta + current, Motive.MIN, Motive.MAX) - current;
                }
                return deltaUtility;
            }
            else if (action is ScheduleAction sAction)
            {
                return GetEffectDeltaForAgentAction(agent, GetActionByName(sAction.InstigatorAction));
            }

            return deltaUtility;
        }
    }
}
