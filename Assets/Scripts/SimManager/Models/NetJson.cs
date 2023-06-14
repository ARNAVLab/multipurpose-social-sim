using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Anthology.Models
{
    public class NetJson : JsonRW 
    {
        public static JsonSerializerOptions Jso { get; } = new()
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true
        };

        public override void InitWorldFromPaths(string pathsFile)
        {
            using FileStream os = File.OpenRead(pathsFile);
            Dictionary<string, string>? filePaths = JsonSerializer.Deserialize<Dictionary<string, string>>(os, Jso);
            if (filePaths == null || filePaths.Count < 3) { throw new FormatException("Unable to load Anthology world state from file"); ; }
            World.Init(filePaths["Actions"], filePaths["Agents"], filePaths["Locations"]);
        }

        public override void LoadActionsFromFile(string path) 
        {
            string actionsText = File.ReadAllText(path);
            ActionContainer? actions = JsonSerializer.Deserialize<ActionContainer>(actionsText, Jso);
            if (actions == null) return;
            ActionManager.Actions = actions;
        }

        public override string SerializeAllActions()
        {
            return JsonSerializer.Serialize(ActionManager.Actions, Jso);
        }

        public override void LoadAgentsFromFile(string path) 
        {
            string agentsText = File.ReadAllText(path);
            List<SerializableAgent>? sAgents = JsonSerializer.Deserialize<List<SerializableAgent>>(agentsText, Jso);

            if (sAgents == null) return;
            foreach (SerializableAgent s in sAgents)
            {
                AgentManager.Agents.Add(SerializableAgent.DeserializeToAgent(s));
            }
        }

        public override string SerializeAllAgents()
        {
            List<SerializableAgent> sAgents = new();
            foreach(Agent a in AgentManager.Agents)
            {
                sAgents.Add(SerializableAgent.SerializeAgent(a));
            }

            return JsonSerializer.Serialize(sAgents, Jso);
        }

        public override void LoadLocationsFromFile(string path) 
        {
            string locationsText = File.ReadAllText(path);
            IEnumerable<SimLocation>? sLocations = JsonSerializer.Deserialize<IEnumerable<SimLocation>>(locationsText, Jso);

            if (sLocations == null) return;
            foreach (SimLocation l in sLocations)
            {
                LocationManager.AddLocation(l);
            }
        }

        public override string SerializeAllLocations()
        {
            static bool HasName(SimLocation simLocation)
            {
                return simLocation.Name != string.Empty;
            }

            IEnumerable<SimLocation> namedLocations = LocationManager.LocationSet.Where(HasName);
            return JsonSerializer.Serialize(namedLocations, Jso);
        }
    }
}