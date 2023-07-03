using System;
using System.Collections.Generic;
using System.Linq;

namespace Anthology.Models
{
    public static class ActionManager
    {
        /** Actions available in the simulation */
        public static ActionContainer Actions { get; set; } = new();

        /** Initialize/reset all action manager variables */
        public static void Init(string path)
        {
            Actions.ScheduleActions.Clear();
            Actions.PrimaryActions.Clear();
            World.ReadWrite.LoadActionsFromFile(path);
        }

        /** Retrieves an action with the specified name from the set of actions available in the simulation */
        public static Action GetActionByName(string actionName)
        {
            bool HasName(Action action)
            {
                return action.Name == actionName;
            }

            Action action;
            try
            {
                action = Actions.PrimaryActions.First(HasName);
            }
            catch (Exception)
            {
                try
                {
                    action = Actions.ScheduleActions.First(HasName);
                }
                catch (Exception)
                {
                    throw new Exception("Action with name: " + actionName + " cannot be found.");
                }
            }
            return action;
        }

        /**
         * Returns the net effect for an action for a specific agent
         * Takes into account the agent's current motivation statuses
         */
        public static float GetEffectDeltaForAgentAction(Agent agent, Action? action)
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
