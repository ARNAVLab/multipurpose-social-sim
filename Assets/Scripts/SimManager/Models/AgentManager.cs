using System;
using System.Collections.Generic;
using System.Linq;

namespace Anthology.Models
{
    public static class AgentManager
    {
        /** Agents in the simulation */
        public static HashSet<Agent> Agents { get; set; } = new HashSet<Agent>();

        /** Initialize/reset all agent manager variables */
        public static void Init(string path)
        {
            Agents.Clear();
            World.ReadWrite.LoadAgentsFromFile(path);
        }

        /** Gets the agent in the simulation with the matching name */
        public static Agent GetAgentByName(string name)
        {
            bool MatchName(Agent a)
            {
                return a.Name == name;
            }

            Agent agent = Agents.First(MatchName);
            return agent;
        }

        /** Check whether the agent satisfies the motive requirement for an action */
        public static bool AgentSatisfiesMotiveRequirement(Agent agent, HashSet<Requirement> reqs)
        {
            foreach (Requirement r in reqs)
            {
                if (r is RMotive rMotive)
                {
                    string t = rMotive.MotiveType;
                    float c = rMotive.Threshold;
                    switch (rMotive.Operation)
                    {
                        case BinOps.EQUALS:
                            if (!(agent.Motives[t].Amount == c))
                            {
                                return false;
                            }
                            break;
                        case BinOps.LESS:
                            if (!(agent.Motives[t].Amount < c))
                            {
                                return false;
                            }
                            break;
                        case BinOps.GREATER:
                            if (!(agent.Motives[t].Amount > c))
                            {
                                return false;
                            }
                            break;
                        case BinOps.LESS_EQUALS:
                            if (!(agent.Motives[t].Amount <= c))
                            {
                                return false;
                            }
                            break;
                        case BinOps.GREATER_EQUALS:
                            if (!(agent.Motives[t].Amount >= c))
                            {
                                return false;
                            }
                            break;
                        default:
                            Console.WriteLine("ERROR - JSON BinOp specification mistake for Motive Requirement for action");
                            return false;
                    }
                }
            }
            return true;
        }

        /**
         * Stopping condition for the simulation
         * Stops the sim when all agents are content 
         */
        public static bool AllAgentsContent()
        {
            foreach (Agent a in Agents)
            {
                if (!a.IsContent()) return false;
            }
            return true;
        }

        /** Decrements the motives of every agent in the simulation */
        public static void DecrementMotives()
        {
            foreach (Agent a in Agents)
            {
                a.DecrementMotives();
            }
        }
    }
}
