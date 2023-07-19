using Anthology.Models;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Anthology.SimulationManager
{
    public class AnthologyRS : RealitySim
    {
        
        public override void Init(string pathFile = "")
        {
            ExecutionManager.Init(pathFile);
        }

        public override void LoadNpcs(Dictionary<string, NPC> npcs)
        {
            HashSet<Agent> agents = AgentManager.Agents;
            foreach (Agent a in agents)
            {
                if (!npcs.TryGetValue(a.Name, out NPC? npc))
                    npc = new NPC();
                npc.Name = a.Name;
                npc.SetCoordinates(a.XLocation, a.YLocation);
                if (a.CurrentAction != null && a.CurrentAction.Count > 0)
                {
                    npc.CurrentAction.Name = a.CurrentAction.First().Name;
                }
                npc.ActionCounter = a.OccupiedCounter;
                if (a.XDestination != -1)
                {
                    npc.Destination = LocationManager.LocationGrid[a.XDestination][a.YDestination].Name;
                }
                Dictionary<string, float> motives = a.Motives;
                foreach (string mote in motives.Keys)
                {
                    npc.Motives[mote] = motives[mote];
                }
                npc.Relationships = a.Relationships;
                npcs[a.Name] = npc;
            }
        }

        public override void LoadLocations(Dictionary<Location.Coords, Location> locations)
        {
            locations.Clear();
            HashSet<SimLocation> simLocations = LocationManager.LocationSet;
            foreach(SimLocation s in simLocations)
            {
                Location loc = new()
                {
                    Name = s.Name,
                    Coordinates = new(s.X, s.Y),
                    Tags = new()
                };
                loc.Tags.UnionWith(s.Tags);
                locations.Add(loc.Coordinates, loc);
            }
        }

        public override void UpdateNpc(NPC npc)
        {
            bool shouldLog = false;
            Agent agent = AgentManager.GetAgentByName(npc.Name);
            npc.SetCoordinates(agent.XLocation, agent.YLocation);

            if (agent.XDestination != -1)
            {
                npc.Destination = LocationManager.LocationGrid[agent.XDestination][agent.YDestination].Name;
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

        public override void PushUpdatedNpc(NPC npc)
        {
            Agent agent = AgentManager.GetAgentByName(npc.Name);
            agent.XLocation = (int)npc.Coordinates.X;
            agent.YLocation = (int)npc.Coordinates.Y;
            Dictionary<string, float> motives = npc.Motives;
            foreach (string mote in motives.Keys)
            {
                agent.Motives[mote] = motives[mote];
            }
        }

        public override void Run(int steps = 1)
        {
            ExecutionManager.RunSim(steps);
        }
    }
}
