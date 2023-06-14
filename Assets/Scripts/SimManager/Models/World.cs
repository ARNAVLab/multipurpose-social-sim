namespace Anthology.Models
{
    public static class World
    {
        /** World time, or ticks */
        public static int Time { get; set; } = 0;

        /** Json parser to use for file I/O, can be swapped from compatibility */
        public static JsonRW ReadWrite { get; set; } = new NetJson();

        /** Initialize/reset all static world variables */
        public static void Init(string actionPath, string agentPath, string locationPath)
        {
            Time = 0;
            ActionManager.Init(actionPath);
            AgentManager.Init(agentPath);
            LocationManager.Init(UI.GridSize, locationPath);
        }

        /** Increment simulation time by one tick */
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
