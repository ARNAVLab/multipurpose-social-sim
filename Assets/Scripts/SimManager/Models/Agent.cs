using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Anthology.Models
{
    /**
     * Relationship class
     * Relationships are composed by agents, so the owning agent will always be the source of the relationship.
     * eg. an agent that has the 'brother' relationship with Norma is Norma's brother
     */
    public class Relationship
    {
        /** The type of relationship, eg. 'student' or 'teacher' */
        public string Type { get; set; } = string.Empty;

        /** The agent that this relationship is with */
        public string With { get; set; } = string.Empty;

        /** How strong the relationship is */
        public float Valence { get; set; }
    }

    /**
     * Agent class
     * Describes the agent object or NPCs in the simulation 
     */
    public class Agent
    {
        /** Name of the agent */
        public string Name { get; set; } = string.Empty;

        /** Container of all the motive properties of this agent */
        public Dictionary<string, float> Motives { get; set; } = new Dictionary<string, float>()
                                                                      {
                                                                        { "accomplishment", 1 },
                                                                        { "emotional", 1 },
                                                                        { "financial", 1 },
                                                                        { "social", 1 },
                                                                        { "physical", 1 } 
                                                                      };

        /** Set of all the relationships of this agent */
        public HashSet<Relationship> Relationships { get; set; } = new HashSet<Relationship>();

        /** Current location of the agent */
        public int XLocation { get; set; }
        public int YLocation { get; set; }

        /** How long the agent will be occupied with the current action they are executing */
        public int OccupiedCounter { get; set; }

        /** A queue containing the next few actions being executed by the agent */
        public LinkedList<Action> CurrentAction { get; set; } = new LinkedList<Action>();

        /**
         * The destination that the agent is heading to
         * Can be -1 if the agent has reached their previous destination and is 
         * executing an action at that location.
         */
        public int XDestination { get; set; } = -1;
        public int YDestination { get; set; } = -1;

        /** List of targets for the agent's current action */
        public HashSet<Agent> CurrentTargets { get; set; } = new HashSet<Agent>();

        /** Starts travel to the agent's destination */
        public void StartTravelToLocation(SimLocation destination, float time)
        {
            XDestination = destination.X;
            YDestination = destination.Y;
            SimLocation currentLoc = LocationManager.LocationGrid[XLocation][YLocation];
            OccupiedCounter = LocationManager.FindManhattanDistance(currentLoc, destination);
/*            Console.WriteLine("time: " + time.ToString() + " | " + Name + ": Started " + CurrentAction.First().Name + "; Destination: " + destination.Name);*/
        }

        /**
         * Move closer to the agent's destination
         * Uses the manhattan distance to move the agent, so either moves along the x or y axis during any tick 
         */
        public void MoveCloserToDestination()
        {
            if (XDestination == -1) return;

            LocationManager.LocationGrid[XLocation][YLocation].AgentsPresent.Remove(Name);

            if (XLocation != XDestination)
            {
                XLocation += XLocation > XDestination ? -1 : 1;
            }
            else if (YLocation != YDestination)
            {
                YLocation += YLocation > YDestination ? -1 : 1;
            }
            else
            {
                XDestination = -1;
                YDestination = -1;
                return;
            }
            LocationManager.LocationGrid[XLocation][YLocation].AgentsPresent.Add(Name);
        }

        /** Applies the effect of an action to this agent */
        public void ExecuteAction()
        {
            XDestination = -1;
            YDestination = -1;
            OccupiedCounter = 0;

            if (CurrentAction.Count > 0)
            {
                Action action = CurrentAction.First();
                CurrentAction.RemoveFirst();

                if (action is PrimaryAction pAction)
                {
                    foreach (KeyValuePair<string, float> e in pAction.Effects)
                    {
                        float delta = e.Value;
                        float current = Motives[e.Key];
                        Motives[e.Key] = Math.Clamp(delta + current, Motive.MIN, Motive.MAX);
                    }
                }
                else if (action is ScheduleAction sAction)
                {
                    if (sAction.Interrupt)
                    {
                        CurrentAction.AddFirst(ActionManager.GetActionByName(sAction.InstigatorAction));
                    }
                    else
                    {
                        CurrentAction.AddLast(ActionManager.GetActionByName(sAction.InstigatorAction));
                    }
                    foreach(Agent target in CurrentTargets)
                    {
                        if (sAction.Interrupt)
                        {
                            target.CurrentAction.AddFirst(ActionManager.GetActionByName(sAction.TargetAction));
                        }
                        else
                        {
                            target.CurrentAction.AddLast(ActionManager.GetActionByName(sAction.TargetAction));
                        }
                    }
                }
            }
        }

        /**
         * Starts an action (if the agent is at a location where the action can be performed)
         * else makes the agent travel to a suitable location to perform the action
         */
        public void StartAction()
        {
            Action action = CurrentAction.First();
            OccupiedCounter = action.MinTime;
            
            if (action is ScheduleAction)
            {
                CurrentTargets.Clear();
                SimLocation currentLoc = LocationManager.LocationGrid[XLocation][YLocation];
                foreach (string name in currentLoc.AgentsPresent)
                {
                    CurrentTargets.Add(AgentManager.GetAgentByName(name));
                }
            }
        }

        /**
         * Selects an action from a set of valid actions to be performed by this agent.
         * Selects the action with the maximal utility of the agent (motive increase / time).
         */
        public void SelectNextAction()
        {
            float maxDeltaUtility = 0f;
            List<Action> currentChoice = new();
            List<SimLocation> currentDest = new();
            List<string> actionSelectLog = new();
            SimLocation currentLoc = LocationManager.LocationGrid[XLocation][YLocation];
            HashSet<Action> actionOptions = new();
            actionOptions.UnionWith(ActionManager.Actions.ScheduleActions);
            actionOptions.UnionWith(ActionManager.Actions.PrimaryActions);

            foreach(Action action in actionOptions)
            {
                if (action.Hidden) continue;
                actionSelectLog.Add("Action: " + action.Name);

                int travelTime;
                HashSet<SimLocation> possibleLocations = new();
                possibleLocations.UnionWith(LocationManager.LocationSet);

                HashSet<Requirement> rMotives = action.GetRequirementsByType(Requirement.MOTIVE);
                HashSet<Requirement> rLocations = action.GetRequirementsByType(Requirement.LOCATION);
                HashSet<Requirement> rPeople = action.GetRequirementsByType(Requirement.PEOPLE);

                if(possibleLocations.Count > 0 && rMotives.Count > 0)
                {
                    if (!AgentManager.AgentSatisfiesMotiveRequirement(this, rMotives))
                    {
                        possibleLocations.Clear();
                    }
                }
                if (possibleLocations.Count > 0 && rLocations.Count > 0)
                {
                    if (rLocations.First() is RLocation rLoc)
                    {
                        possibleLocations = LocationManager.LocationsSatisfyingLocationRequirement(possibleLocations, rLoc);
                    }
                }
                if (possibleLocations.Count > 0 && rPeople.Count > 0)
                {
                    if (rPeople.First() is RPeople rPpl)
                    {
                        possibleLocations = LocationManager.LocationsSatisfyingPeopleRequirement(possibleLocations, rPpl);
                    }
                }

                if (possibleLocations.Count > 0)
                {
                    SimLocation? nearestLocation = LocationManager.FindNearestLocationFrom(possibleLocations, this);
                    if (nearestLocation == null) continue;
                    travelTime = LocationManager.FindManhattanDistance(nearestLocation, currentLoc);
                    float deltaUtility = ActionManager.GetEffectDeltaForAgentAction(this, action);
/*                    actionSelectLog.Add("Action Utility: " + deltaUtility.ToString());*/
                    deltaUtility /= (action.MinTime + travelTime);
/*                    actionSelectLog.Add("Action Weighted Utility: " + deltaUtility.ToString());*/

                    if (deltaUtility == maxDeltaUtility)
                    {
                        currentChoice.Add(action);
                        currentDest.Add(nearestLocation);
                    }
                    else if (deltaUtility > maxDeltaUtility)
                    {
                        maxDeltaUtility = deltaUtility;
                        currentChoice.Clear();
                        currentDest.Clear();
                        currentChoice.Add(action);
                        currentDest.Add(nearestLocation);
                    }
/*                    if (currentChoice.Count > 0)
                    {
                        actionSelectLog.Add("Current Choice: " + currentChoice.First().Name);
                        actionSelectLog.Add("Current Destination: " + currentDest.First().Name);
                    }*/
                }
            }
            Random r = new();
            int idx = r.Next(0, currentChoice.Count);
            Action choice = currentChoice[idx];
            SimLocation dest = currentDest[idx];
            CurrentAction.AddLast(choice);
            

            if (dest != null && dest != currentLoc)
            {
                CurrentAction.AddFirst(ActionManager.GetActionByName("travel_action"));
                StartTravelToLocation(dest, World.Time);
            }
            else if (dest == null || dest == currentLoc)
            {
                StartAction();
            }
        }

        /** Returns whether the agent is content, ie. checks to see if an agent has the maximum motives */
        public bool IsContent()
        {
            foreach (float m in Motives.Values)
            {
                if (m < Motive.MAX) return false;
            }
            return true;
        }

        /** Decrements all the motives of this agent */
        public void DecrementMotives()
        {
            foreach(string m in Motives.Keys)
            {
                Motives[m] = Math.Clamp(Motives[m] - 1, Motive.MIN, Motive.MAX);
            }
        }
    }

    /**
     * Agent class received from a JSON file
     * The action is provided as a string and matched to the Agent.CurrentAction object accordingly
     */
    public class SerializableAgent
    {
        /** initialized to the name of the agent */
        public string Name { get; set; } = string.Empty;

        /** motives intiailized with values for the agent */
        public Dictionary<string, float> Motives { get; set; } = new();

        /** starting X coordinate */
        public int XLocation { get; set; }

        /** starting Y coordinate */
        public int YLocation { get; set; }

        /** describes whether the agent is currently occupied */
        public int OccupiedCounter { get; set; }

        /** queue containing the next few actions being executed by the agent */
        public string CurrentAction { get; set; } = string.Empty;

        /** Location the agent is currently headed to */
        public int XDestination { get; set; } = -1;
        public int YDestination { get; set; } = -1;

        /** list of targets for the agent's current action */
        public List<string> CurrentTargets { get; set; } = new List<string>();

        /** list of relationships that the agent possesses */
        public List<Relationship> Relationships { get; set; } = new List<Relationship>();

        /** Creates a serializable agent from the given agent for file I/O */
        public static SerializableAgent SerializeAgent(Agent agent)
        {
            SerializableAgent serializableAgent = new()
            {
                Name = agent.Name,
                Motives = new(),
                XLocation = agent.XLocation,
                YLocation = agent.YLocation,
                OccupiedCounter = agent.OccupiedCounter,
                CurrentAction = string.Empty,
                XDestination = agent.XDestination,
                YDestination = agent.YDestination,
                Relationships = new()
            };

            if (agent.CurrentAction.Count > 0)
            {
                serializableAgent.CurrentAction = agent.CurrentAction.First().Name;
            }
            else
            {
                serializableAgent.CurrentAction = "wait_action";
            }

            foreach (Relationship r in agent.Relationships)
            {
                serializableAgent.Relationships.Add(r);
            }

            foreach(KeyValuePair<string, float> m in agent.Motives)
            {
                serializableAgent.Motives.Add(m.Key, m.Value);
            }

            return serializableAgent;
        }

        /** Creates an agent from the given serializable agent for file I/O */
        public static Agent DeserializeToAgent(SerializableAgent sAgent)
        {
            Agent agent = new()
            {
                Name = sAgent.Name,
                XLocation = sAgent.XLocation,
                YLocation = sAgent.YLocation,
                OccupiedCounter = sAgent.OccupiedCounter,
                XDestination = sAgent.XDestination,
                YDestination = sAgent.YDestination,
            };

            agent.CurrentAction.AddFirst(ActionManager.GetActionByName(sAgent.CurrentAction));

            foreach (KeyValuePair<string, float> e in sAgent.Motives)
            {
                agent.Motives[e.Key] = e.Value;
            }
            foreach (Relationship r in sAgent.Relationships)
            {
                agent.Relationships.Add(r);
            }
            foreach (string name in sAgent.CurrentTargets)
            {
                agent.CurrentTargets.Add(AgentManager.GetAgentByName(name));
            }

            return agent;
        }
    }
}
