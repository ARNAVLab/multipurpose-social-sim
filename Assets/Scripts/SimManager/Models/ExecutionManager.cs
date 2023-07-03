using System;
using System.Linq;
using System.Text.Json;

namespace Anthology.Models
{
    public static class ExecutionManager
    {
        /** Initializes the simulation, delegates to World.Init() */
        public static void Init(string pathToFiles)
        {
            World.ReadWrite.InitWorldFromPaths(pathToFiles);
        }

        /**
         * Executes a turn for each agent every tick.
         * Executes a single turn and then must be called again
         */
        public static void RunSim(int steps = 1)
        {
            for (int i = 0; i < steps; i++)
            {
                if (ToContinue())
                {
                    foreach (Agent agent in AgentManager.Agents)
                        Turn(agent);

                    World.IncrementTime();
                }
                else if(!UI.Paused)
                {
                    Console.WriteLine("Simulation ended.");
                }
            }

            UI.Update();
        }

        /**
         * Tests whether the simulation should continue.
         * First checks whether the stopping function for the simulation has been met.
         * Next checks if the user has paused the simulation
         */
        public static bool ToContinue()
        {
            if (AgentManager.AllAgentsContent())
            {
                return false;
            }
            else if (!UI.Paused)
            {
                return false;
            }
            return true;
        }

        /**
         * Updates movement and occupation counters for an agent
         * May decrement the motives of an agent once every 10 hours. Chooses or executes an action when necessary.
         */
        public static bool Turn(Agent agent)
        {
            bool movement = false;
/*            Console.WriteLine(agent.Name);*/
            if (agent.OccupiedCounter > 0)
            {
                agent.OccupiedCounter--;

                if (agent.CurrentAction.First().Name == "travel_action" && agent.XDestination != -1)
                {
                    movement = true;
                    agent.MoveCloserToDestination();
                }
            }
            // If not travelling (i.e. arrived at destination), and end of occupied, execute planned action effects, select/start next.
            else
            {
                agent.ExecuteAction();
                if (!agent.IsContent())
                {
                    if (agent.CurrentAction.Count == 0)
                    {
                        agent.SelectNextAction();
                    }
                    else
                    {
                        agent.StartAction();
                    }
                }
            }
            return movement;
        }

        /**
         * Interrupts the agent from the current action they are performing.
         * Potential future implementation: Optionally add the interrupted action (with the remaining occupied counter) to the end of the action queue.
         */
        public static void Interrupt(Agent agent)
        {
            agent.OccupiedCounter = 0;
            agent.XDestination = -1;
            agent.YDestination = -1;
            Action interrupted = agent.CurrentAction.First();
            agent.CurrentAction.RemoveFirst();
            Console.WriteLine("Agent: " + agent.Name + " was interrupted from action: " + interrupted.Name);
        }

        public static void Interrupt(string agentName)
        {
            Agent? agent = AgentManager.GetAgentByName(agentName);
            if (agent != null)
            {
                Interrupt(agent);
            }
        }
    }
}
