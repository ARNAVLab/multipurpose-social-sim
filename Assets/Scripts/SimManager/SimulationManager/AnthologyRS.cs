using Anthology.Models;
using System.Collections.Generic;
using System.Linq;

namespace Anthology.SimulationManager
{
    /// <summary>
    /// Concrete implementation of the reality sim using Anthology.
    /// </summary>
    public class AnthologyRS : RealitySim
    {
        /// <summary>
        /// Initalizes Anthology with given path to JSON file.
        /// </summary>
        /// <param name="pathFile">Path to JSON file containing relevant info for initializing Anthology.</param>
        public override void Init(string pathFile = "")
        {
            ExecutionManager.Init(pathFile);
        }

        /// <summary>
        /// Loads all NPCs from Anthology to given dictionary.
        /// </summary>
        /// <param name="npcs">The dictionary to populate.</param>
        public override void LoadNpcs(Dictionary<string, NPC> npcs)
        {
            List<Agent> agents = AgentManager.Agents;
            foreach (Agent a in agents)
            {
                if (!npcs.TryGetValue(a.Name, out NPC? npc))
                    npc = new NPC();
                npc.Name = a.Name;
                npc.Location = a.CurrentLocation;
                if (a.CurrentAction != null && a.CurrentAction.Count > 0)
                {
                    npc.CurrentAction.Name = a.CurrentAction.First().Name;
                }
                npc.ActionCounter = a.OccupiedCounter;
                if (a.Destination != string.Empty)
                {
                    npc.Destination = a.Destination;
                }
                Dictionary<string, float> motives = a.Motives;
                foreach (string mote in motives.Keys)
                {
                    npc.Motives[mote] = motives[mote];
                }
                npcs[a.Name] = npc;
            }
        }

        /// <summary>
        /// Loads all locations from Anthology to given dictionary.
        /// </summary>
        /// <param name="locations">The dictionary to populate.</param>
        public override void LoadLocations(Dictionary<Location.Coords, Location> locations)
        {
            locations.Clear();
            IEnumerable<LocationNode> locNodes = LocationManager.LocationsByName.Values;
            foreach(LocationNode locNode in locNodes)
            {
                Location loc = new()
                {
                    Name = locNode.Name,
                    Coordinates = new(locNode.X, locNode.Y),
                };
                loc.Tags.UnionWith(locNode.Tags);
                foreach(KeyValuePair<string, float> con in locNode.Connections)
                {
                    loc.Connections.Add(con.Key, con.Value);
                }
                locations.Add(loc.Coordinates, loc);
            }
        }

        /// <summary>
        /// Updates Anthology's locations to match SimManager's.
        /// </summary>
        public override void PushLocations()
        {
            /*LocationManager.LocationSet.Clear();
            LocationManager.LocationGrid.Clear();
            UI.GridSize = 0;
            foreach (Location loc in SimManager.Locations.Values)
            {
                LocationManager.AddLocation(loc.Name, loc.Coordinates.X, loc.Coordinates.Y, loc.Tags);
            }*/
        }

        /// <summary>
        /// Updates given SimManager's NPC to match Anthology's NPC.
        /// </summary>
        /// <param name="npc">The SimManager's NPC to update.</param>
        public override void UpdateNpc(NPC npc)
        {
            bool shouldLog = false;
            Agent agent = AgentManager.GetAgentByName(npc.Name);
            npc.Location = agent.CurrentLocation;

            if (agent.Destination != string.Empty)
            {
                npc.Destination = agent.Destination;
            }
            else
            {
                npc.Destination = string.Empty;
            }
            Dictionary<string, float> motives = agent.Motives;
            foreach (string mote in motives.Keys)
            {
                if (!npc.Motives.ContainsKey(mote))
                {
                    npc.Motives[mote] = motives[mote];
                }
                else if (npc.Motives[mote] != motives[mote]) {
                    shouldLog |= true;
                    npc.Motives[mote] = motives[mote];
                }
            }
            if (agent.CurrentAction.Count > 0 && npc.CurrentAction.Name != agent.CurrentAction.First().Name)
            {
                shouldLog = true;
                npc.CurrentAction.Name = agent.CurrentAction.First().Name;
            }
            npc.ActionCounter = agent.OccupiedCounter;
            if (shouldLog)
            {
                SimManager.History?.AddNpcToLog(npc);
            }
        }

        /// <summary>
        /// Updates Anthology's NPC with given SimManager's NPC.
        /// </summary>
        /// <param name="npc">The SimManager's NPC to be used for updating.</param>
        public override void PushUpdatedNpc(NPC npc)
        {
            Agent agent = AgentManager.GetAgentByName(npc.Name);
            agent.CurrentLocation = npc.Location;
            Dictionary<string, float> motives = npc.Motives;
            foreach (string mote in motives.Keys)
            {
                agent.Motives[mote] = motives[mote];
            }
        }

        /// <summary>
        /// Run Anthology sim by given amount of steps.
        /// </summary>
        /// <param name="steps">Number of steps to advance Anthology by.</param>
        public override void Run(int steps = 1)
        {
            ExecutionManager.RunSim(steps);
        }
    }
}
