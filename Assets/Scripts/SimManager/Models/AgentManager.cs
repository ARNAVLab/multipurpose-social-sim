using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Anthology.Models
{
    /// <summary>
    /// Manages agents and their motives.
    /// </summary>
    public static class AgentManager
    {
        /// <summary>
        /// Agents in the simulation.
        /// </summary>
        public static List<Agent> Agents { get; set; } = new();

        /// <summary>
        /// Initializes/resets all agent manager variables.
        /// </summary>
        /// <param name="path">Path of JSON file to load from.</param>
        public static void Init(string path)
        {
            Agents.Clear();
            World.ReadWrite.LoadAgentsFromFile(path);
        }

        /// <summary>
        /// Removes all agents from the simulation.
        /// </summary>
        public static void Reset()
        {
            foreach (LocationNode loc in LocationManager.LocationsByName.Values)
            {
                loc.AgentsPresent.Clear();
            }
            Agents.Clear();
        }

        /// <summary>
        /// Adds the given agent to the simulation and marks it as present in its current location
        /// </summary>
        /// <param name="agent">The agent to add to the simulation</param>
        public static void AddAgent(Agent agent)
        {
            Agents.Add(agent);
            if (LocationManager.LocationsByName.ContainsKey(agent.CurrentLocation))
            {
                LocationManager.LocationsByName[agent.CurrentLocation].AgentsPresent.AddLast(agent.Name);
            }
        }

        /// <summary>
        /// Gets the agent in the simulation with the matching name.
        /// </summary>
        /// <param name="name">Name of the agent to retrieve.</param>
        /// <returns>Agent with given name.</returns>
        public static Agent GetAgentByName(string name)
        {
            bool MatchName(Agent a)
            {
                return a.Name == name;
            }
            Agent? agent = Agents.Find(MatchName);
            return agent ?? throw new ArgumentException("Agent with name: " + name + " does not exist.");
        }

        /// <summary>
        /// Checks whether the agent satisfies the motive requirement for an action.
        /// </summary>
        /// <param name="agent">The agent to check.</param>
        /// <param name="reqs">The requirements to check.</param>
        /// <returns>True if agent satisfies all requirements for an action.</returns>
        public static bool AgentSatisfiesMotiveRequirement(Agent agent, IEnumerable<RMotive> reqs)
        {
            foreach (RMotive r in reqs)
            {
                string t = r.MotiveType;
                float c = r.Threshold;
                switch (r.Operation)
                {
                    case BinOps.EQUALS:
                        if (!(agent.Motives[t] == c))
                        {
                            return false;
                        }
                        break;
                    case BinOps.LESS:
                        if (!(agent.Motives[t] < c))
                        {
                            return false;
                        }
                        break;
                    case BinOps.GREATER:
                        if (!(agent.Motives[t] > c))
                        {
                            return false;
                        }
                        break;
                    case BinOps.LESS_EQUALS:
                        if (!(agent.Motives[t] <= c))
                        {
                            return false;
                        }
                        break;
                    case BinOps.GREATER_EQUALS:
                        if (!(agent.Motives[t] >= c))
                        {
                            return false;
                        }
                        break;
                    default:
                        Console.WriteLine("ERROR - JSON BinOp specification mistake for Motive Requirement for action");
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Stopping condition for the simulation.
        /// Stops the sim when all agents are content.
        /// </summary>
        /// <returns>True if all agents are content.</returns>
        public static bool AllAgentsContent()
        {
            foreach (Agent a in Agents)
            {
                if (!a.IsContent()) return false;
            }
            return true;
        }

        /// <summary>
        /// Decrements the motives of every agent in the simulation.
        /// </summary>
        public static void DecrementMotives()
        {
            Parallel.ForEach(Agents, a =>
            {
                a.DecrementMotives();
            });
        }
    }
}
