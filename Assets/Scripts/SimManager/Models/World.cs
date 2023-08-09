namespace Anthology.Models
{
    /// <summary>
    /// Simulation environment that encompasses the real world.
    /// </summary>
    public static class World
    {
        /// <summary>
        /// World time, or ticks.
        /// </summary>
        public static int Time { get; set; } = 0;

        /// <summary>
        /// Json parser to use for file I/O, can be swapped from compatibility.
        /// </summary>
        public static JsonRW ReadWrite { get; set; } = new NetJson();

        /// <summary>
        /// Initialize/reset all static world variables.
        /// </summary>
        /// <param name="actionPath">Path of JSON file to load actions from.</param>
        /// <param name="agentPath">Path of JSON file to load agents from.</param>
        /// <param name="locationPath">Path of JSON file to load locations from.</param>
        public static void Init(string actionPath, string agentPath, string locationPath)
        {
            Time = 0;
            ActionManager.Init(actionPath);
            AgentManager.Init(agentPath);
            LocationManager.Init(locationPath);
        }

        /// <summary>
        /// Increment simulation time by one tick.
        /// </summary>
        public static void IncrementTime()
        {
            Time += 1;
            if (Time % 1200 == 0)
            {
                foreach (Agent agent in AgentManager.Agents)
                {
                    if (!agent.IsContent())
                    {
                        agent.DecrementMotives();
                    }
                }
            }
        }
    }
}
