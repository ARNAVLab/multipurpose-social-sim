using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Anthology.Models
{
    /// <summary>
    /// Manages execution and lifetime of the simulation.
    /// </summary>
    public static class ExecutionManager
    {
        /// <summary>
        /// Initializes the simulation by initializing the world.
        /// </summary>
        /// <param name="pathToFiles">Path of JSON file containing relevant paths to other JSON files.</param>
        public static void Init(string pathToFiles)
        {
            World.ReadWrite.InitWorldFromPaths(pathToFiles);
        }

        /// <summary>
        /// Executes a turn for each agent every tick.
        /// Executes a single turn and then must be called again.
        /// </summary>
        /// <param name="steps">Number of steps to advance the simulation by.</param>
        public static void RunSim(int steps = 1)
        {
            for (int i = 0; i < steps; i++)
            {
                if (ToContinue())
                {
                    Parallel.ForEach(AgentManager.Agents, agent =>
                    {
                        Turn(agent);
                    });

                    World.IncrementTime();
                }
                else if(!UI.Paused)
                {
                    Console.WriteLine("Simulation ended.");
                }
            }

            UI.Update();
        }

        /// <summary>
        /// Tests whether the simulation should continue.
        /// First checks whether the stopping function for the simulation has been met.
        /// Next checks if the user has paused the simulation.
        /// </summary>
        /// <returns>True if the simulation should continue.</returns>
        public static bool ToContinue()
        {
            return !(UI.Paused || AgentManager.AllAgentsContent());
        }

        /// <summary>
        /// Updates movement and occupation counters for an agent.
        /// May decrement the motives of an agent once every 10 hours. Chooses or executes an action when necessary.
        /// </summary>
        /// <param name="agent">The agent whose turn will happen.</param>
        /// <returns>True if agent moved from original position.</returns>
        public static bool Turn(Agent agent)
        {
            bool movement = false;
/*            Console.WriteLine(agent.Name);*/
            if (agent.OccupiedCounter > 0)
            {
                agent.OccupiedCounter--;

                if (agent.CurrentAction.First().Name == "travel_action" && agent.Destination != string.Empty)
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

        /// <summary>
        /// Interrupts the agent from the current action they are performing.
        /// Potential future implementation: Optionally add the interrupted action (with the remaining occupied counter) to the end of the action queue.
        /// </summary>
        /// <param name="agent">The agent to interrupt.</param>
        public static void Interrupt(Agent agent)
        {
            agent.OccupiedCounter = 0;
            agent.Destination = string.Empty;
            Action interrupted = agent.CurrentAction.First();
            agent.CurrentAction.RemoveFirst();
            Console.WriteLine("Agent: " + agent.Name + " was interrupted from action: " + interrupted.Name);
        }

        /// <summary>
        /// Interrupt the agent whose name matches given name.
        /// </summary>
        /// <param name="agentName">The name of the agent to interrupt.</param>
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
