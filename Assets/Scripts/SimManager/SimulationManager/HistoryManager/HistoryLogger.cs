using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Anthology.SimulationManager.HistoryManager
{
    public abstract class HistoryLogger
    {
        public EventLog ELog { get; set; } = new();

        public abstract void AddNpcToLog(NPC npc);

        public abstract void LogNpcStates(string? destination);

        public abstract void SaveState(string stateName);

        public abstract SimState LoadState(string stateName);

        public abstract void DeleteState(string stateName);

        public abstract void ClearStates();

        public abstract void ClearLog(string log);
    }

    public class EventLog
    {
        [BsonId]
        public uint TimeStep { get; set; } = SimManager.NumIterations;

        public Dictionary<string, NPC> NpcChanges { get; set; } = new();
    }

    public class SimState
    {
        [BsonId]
        public string SimName { get; set; }

        public HashSet<NPC> NPCs { get; set; }

        public HashSet<Location> Locations { get; set; }

        public SimState(string name = "")
        {
            SimName = name == "" ? "Sim " + DateTime.Now : name;
            NPCs = new();
            NPCs.UnionWith(SimManager.NPCs.Values);
            Locations = new();
            Locations.UnionWith(SimManager.Locations.Values);
        }
    }
}
