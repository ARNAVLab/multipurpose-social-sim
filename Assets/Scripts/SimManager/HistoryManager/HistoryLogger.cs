using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using SimManager.SimulationManager;

namespace SimManager.HistoryManager
{
    /// <summary>
    /// Logs history of simulation. Manages a log of events that occurred in
    /// the simulation along with simulation states that can be saved/loaded.
    /// </summary>
    public abstract class HistoryLogger
    {
        /// <summary>
        /// Log of events to store.
        /// </summary>
        public EventLog ELog { get; set; } = new();

        /// <summary>
        /// Registers an NPC to be logged.
        /// </summary>
        /// <param name="npc">The NPC to log.</param>
        public abstract void AddNpcToLog(NPC npc);

        /// <summary>
        /// Push NPC state to the log given destination.
        /// </summary>
        /// <param name="destination">Where to store npc state data.</param>
        public abstract void LogNpcStates(string destination);

        /// <summary>
        /// Save the current simulation state with given name.
        /// </summary>
        /// <param name="stateName">Name of the simulation state.</param>
        public abstract void SaveState(string stateName);

        /// <summary>
        /// Loads a simulation state and updates the simulation with contents of given state. 
        /// </summary>
        /// <param name="stateName">Name of the state to load.</param>
        /// <returns></returns>
        public abstract SimState LoadState(string stateName);

        /// <summary>
        /// Deletes a simulation state.
        /// </summary>
        /// <param name="stateName">The name of the simulation state to delete.</param>
        public abstract void DeleteState(string stateName);

        /// <summary>
        /// Clear all simulation states. 
        /// </summary>
        public abstract void ClearStates();

        /// <summary>
        /// Clear all entries in the event log.
        /// </summary>
        /// <param name="log">Name of log to clear.</param>
        public abstract void ClearLog(string log);

        ///<summary>
        ///Exports history log to a .json file
        ///</summary>
        public abstract void ExportCollection();

        public abstract List<BsonDocument> GetActorJson(string actorName);

        public abstract string JsonToNPCLog(List<BsonDocument> list, string actorName);
    }

    /// <summary>
    /// Manages a record of all changes made to NPCs.
    /// </summary>
    public class EventLog
    {
        /// <summary>
        /// Delta steps to advance simulation.
        /// </summary>
        [BsonId]
        public uint TimeStep { get; set; } = SimEngine.NumIterations;

        /// <summary>
        /// Dictionary that keeps track of NPC state changes.
        /// </summary>
        public Dictionary<string, NPC> NpcChanges { get; set; } = new();
    }

    /// <summary>
    /// Maintains all contents of the simulation at a certain time point.
    /// Can be saved/loaded in order to replay the simulation at a certain 
    /// point in time.
    /// </summary>
    public class SimState
    {
        /// <summary>
        /// Name of the state.
        /// </summary>
        [BsonId]
        public string SimName { get; set; }

        /// <summary>
        /// All saved NPCs of the simulation.
        /// </summary>
        public HashSet<NPC> NPCs { get; set; }

        /// <summary>
        /// All saved locations of the simulation.
        /// </summary>
        public HashSet<Location> Locations { get; set; }

        /// <summary>
        /// Constructs a sim state and saves the current state of 
        /// the simulation.
        /// </summary>
        /// <param name="name">Name of the sim state. If empty, uses DateTime.Now.</param>
        public SimState(string name = "")
        {
            SimName = name == "" ? "Sim " + DateTime.Now : name;
            NPCs = new();
            NPCs.UnionWith(SimEngine.NPCs.Values);
            Locations = new();
            Locations.UnionWith(SimEngine.Locations.Values);
        }
    }
}
