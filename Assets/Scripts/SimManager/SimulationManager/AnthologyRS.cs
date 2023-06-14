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
                npc.Coordinates.X = a.XLocation;
                npc.Coordinates.Y = a.YLocation;
                if (a.CurrentAction != null && a.CurrentAction.Count > 0)
                {
                    npc.CurrentAction.Name = a.CurrentAction.First().Name;
                }
                Dictionary<string, Motive> motives = a.Motives;
                foreach (string mote in motives.Keys)
                {
                    npc.Motives[mote] = motives[mote].Amount;
                }
                npcs[a.Name] = npc;
            }
        }

        public override void LoadLocations(Dictionary<Vector2, Location> locations)
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
            Agent agent = AgentManager.GetAgentByName(npc.Name);
            npc.Coordinates.X = agent.XLocation;
            npc.Coordinates.Y = agent.YLocation;
            Dictionary<string, Motive> motives = agent.Motives;
            foreach (string mote in motives.Keys)
            {
                npc.Motives[mote] = motives[mote].Amount;
            }
            if (agent.CurrentAction.Count > 0)
                npc.CurrentAction.Name = agent.CurrentAction.First().Name;
        }

        public override void PushUpdatedNpc(NPC npc)
        {
            Agent agent = AgentManager.GetAgentByName(npc.Name);
            agent.XLocation = (int)npc.Coordinates.X;
            agent.YLocation = (int)npc.Coordinates.Y;
            Dictionary<string, float> motives = npc.Motives;
            foreach (string mote in motives.Keys)
            {
                agent.Motives[mote].Amount = motives[mote];
            }
        }

        public override void Run(int steps = 1)
        {
            ExecutionManager.RunSim(steps);
        }
    }
}
