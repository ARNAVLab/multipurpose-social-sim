using Anthology.Models;
using System.Collections.Generic;
using System.Linq;

namespace SimManager.SimulationManager
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
            foreach (Agent a in AgentManager.Agents.Values)
            {
                if (!npcs.TryGetValue(a.Name, out NPC npc))
                    npc = new NPC();
                npc.Name = a.Name;
                npc.Location = SimEngine.Locations[a.CurrentLocation.Name];
                if (a.CurrentAction != null && a.CurrentAction.Count > 0)
                {
                    npc.CurrentAction.Name = a.CurrentAction.First().Name;
                }
                npc.ActionCounter = a.OccupiedCounter;
                if (a.Destination.Any())
                {
                    npc.Destination = a.Destination.Last();
                }
                Dictionary<string, float> motives = a.Motives.ToDictionary();
                foreach (string mote in motives.Keys)
                {
                    npc.Motives[mote] = motives[mote];
                }
                foreach (Relationship r in a.Relationships)
                {
                    npc.Relationships.Add((Relationship)r);
                }
                npcs[a.Name] = npc;
            }
        }

        /// <summary>
        /// Loads all locations from Anthology to given dictionary.
        /// </summary>
        /// <param name="locations">The dictionary to populate.</param>
        public override void LoadLocations(Dictionary<string, Location> locations)
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
                loc.Tags = locNode.Tags;
                foreach(KeyValuePair<LocationNode, float> con in locNode.Connections)
                {
                    loc.Connections.Add(con.Key.Name, con.Value);
                }
                locations.Add(loc.Name, loc);
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
            npc.Location.AgentsPresent.Remove(npc.Name);
            npc.Location = SimEngine.Locations[agent.CurrentLocation.Name];
            npc.Location.AgentsPresent.Add(npc.Name);

            if (agent.Destination.Any())
            {
                npc.Destination = agent.Destination.Last();
            }
            else
            {
                npc.Destination = string.Empty;
            }
            Dictionary<string, float> motives = agent.Motives.ToDictionary();
            foreach (string mote in motives.Keys)
            {
                if (!npc.Motives.ContainsKey(mote))
                {
                    npc.Motives[mote] = motives[mote];
                }
                else if (npc.Motives[mote] != motives[mote])
                {
                    shouldLog |= true;
                    npc.Motives[mote] = motives[mote];
                }
            }
            if (agent.CurrentAction.Any() && npc.CurrentAction.Name != agent.CurrentAction.First().Name)
            {
                shouldLog = true;
                npc.CurrentAction.Name = agent.CurrentAction.First().Name;
            }
            npc.ActionCounter = agent.OccupiedCounter;
            if (shouldLog)
            {
                SimEngine.History?.AddNpcToLog(npc);
            }
        }

        /// <summary>
        /// Updates Anthology's NPC with given SimManager's NPC.
        /// </summary>
        /// <param name="npc">The SimManager's NPC to be used for updating.</param>
        public override void PushUpdatedNpc(NPC npc)
        {
            Agent agent = AgentManager.GetAgentByName(npc.Name);
            agent.CurrentLocation = LocationManager.LocationsByName[npc.Location.Name];
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

        /// <summary>
        /// Updates locations based on dirty locations.
        /// </summary>
        public override void UpdateLocations()
        {
            foreach (string locName in LocationManager.dirtyLocations)
            {
                SimEngine.Locations[locName].Tags = LocationManager.LocationsByName[locName].Tags;
            }
            LocationManager.dirtyLocations = new();
        }

        // --------------------------------------------------------
        // ADDED: Method to start a communication action and log it
        // --------------------------------------------------------
        /// <summary>
        /// Initiates a communication action between two agents and logs it to the journal JSON.
        /// </summary>
        /// <param name="initiatorName">Name of the agent initiating the communication.</param>
        /// <param name="targetName">Name of the target agent.</param>
        /// <param name="topic">Topic or reason for communicating.</param>
        public void StartCommunicationBetweenAgents(string initiatorName, string targetName, string topic)
        {
            // 1) Get the agent
            Agent initiator = AgentManager.GetAgentByName(initiatorName);
            if (initiator == null)
            {
                UnityEngine.Debug.LogWarning($"Initiator agent '{initiatorName}' not found.");
                return;
            }

            // 2) Retrieve the new "communicate_action"
            // Explicitly use Anthology.Models.Action for clarity.
            Anthology.Models.Action communicateAction;
            try
            {
                communicateAction = ActionManager.GetActionByName("communicate_action");
            }
            catch
            {
                UnityEngine.Debug.LogWarning("communicate_action not found in ActionManager. Did you add it?");
                return;
            }

            // 3) Clear any existing action and set the new one.
            initiator.CurrentAction.Clear();
            initiator.CurrentAction.AddFirst(communicateAction);

            // 4) Log the communication to the journal (if an AgentJournalManager exists in the scene)
            AgentJournalManager journalManager = UnityEngine.Object.FindObjectOfType<AgentJournalManager>();
            if (journalManager != null)
            {
                List<Turn> turns = new List<Turn>()
                {
                    new Turn { speaker = initiatorName, dialog = $"Hello {targetName}, let's talk about {topic}!" },
                    new Turn { speaker = targetName, dialog = "Sure, I'm listening..." }
                };

                journalManager.AddConversation("communication", targetName, topic, turns, System.DateTime.Now);
                UnityEngine.Debug.Log($"Logged conversation between {initiatorName} and {targetName} about {topic}.");
            }
            else
            {
                UnityEngine.Debug.LogWarning("No AgentJournalManager found in the scene. Conversation not logged.");
            }
        }
    }
}
